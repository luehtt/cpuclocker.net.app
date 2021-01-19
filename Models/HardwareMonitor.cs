using CPUClocker.Common;
using CPUClocker.Services;
using InfluxDB.LineProtocol.Payload;
using Newtonsoft.Json;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUClocker.Models
{
    public class HardwareMonitor
    {
        public HardwareMonitor(int interval, int timeOut, string machineName, string userName, List<string> selectedHardware)
        {
            Interval = interval;
            TimeOut = timeOut * 1000 * 60;
            MachineName = machineName;
            UserName = userName;
            SelectedHardware = selectedHardware;
            Computer = new Computer();
        }

        public int Interval { get; set; } // ms
        public int TimeOut { get; set; } // ms
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public List<string> SelectedHardware { get; private set; }
        public Computer Computer { get; private set; }


        public void LoadKafka()
        {
            if (AppConfig.UsingKafka == false) return;
            // task run monitor
            Task.Run(async () => {
                while (true)
                {
                    KafkaService.ConsumeMessage();
                    await Task.Delay(Interval);
                }
            });
        }

        public void StartMonitor()
        {
            Console.WriteLine("Open hardware...");
            Computer.Open();

            // enable modules for selected hardware
            foreach (var hardware in SelectedHardware)
            {
                switch (hardware)
                {
                    case Const.INFLUX_CPU:
                        Console.WriteLine("Enabling CPU...");
                        Computer.CPUEnabled = true;
                        break;
                    case Const.INFLUX_RAM:
                        Console.WriteLine("Enabling RAM...");
                        Computer.RAMEnabled = true;
                        break;
                    case Const.INFLUX_GPU:
                        Console.WriteLine("Enabling GPU...");
                        Computer.GPUEnabled = true;
                        break;
                    case Const.INFLUX_HDD:
                        Console.WriteLine("Enabling HDD...");
                        Computer.HDDEnabled = true;
                        break;
                    case Const.INFLUX_MAIN:
                        Console.WriteLine("Enabling Mainboard...");
                        Computer.MainboardEnabled = true;
                        break;
                }
            }

            // task run monitor
            Task.Run(async () => {
                var timeCount = 0;
                while (timeCount < TimeOut)
                {
                    if (timeCount % 60_000 == 0) Console.WriteLine($"Time ellapsed: {timeCount / 60_000} min");

                    if (Computer.CPUEnabled == true) Monitor(HardwareType.CPU);
                    if (Computer.RAMEnabled == true) Monitor(HardwareType.RAM);
                    if (Computer.GPUEnabled == true) Monitor(HardwareType.GpuAti);
                    if (Computer.GPUEnabled == true) Monitor(HardwareType.GpuNvidia);
                    if (Computer.HDDEnabled == true) Monitor(HardwareType.HDD);
                    if (Computer.MainboardEnabled == true) Monitor(HardwareType.Mainboard);

                    await Task.Delay(Interval);
                    timeCount += Interval;
                }
            });
        }

        private void Monitor(HardwareType hardwareType)
        {
            var hardwares = Computer.Hardware.Where(x => x.HardwareType == hardwareType);
            var infos = new List<HardwareInfo>();
            foreach (var hardware in hardwares)
            {
                hardware.Update();
                var info = new HardwareInfo(MachineName, UserName, hardware);
                infos.Add(info);
            }
            
            if (AppConfig.UsingKafka) MonitorKafka(infos);
            else MonitorInflux(infos);
        }

        private void MonitorInflux(IEnumerable<HardwareInfo> infos)
        {
            var list = new List<LineProtocolPoint>();
            foreach (var info in infos)
            {
                var point = InfluxService.ParseInfoToInfluxPoint(info);
                if (point == null) continue;
                list.Add(point);
            }
            _ = InfluxService.UploadInflux(list);
        }

        private void MonitorKafka(IEnumerable<HardwareInfo> infos)
        {
            var list = new List<string>();
            foreach (var info in infos)
            {
                var point = InfluxService.ParseInfoToInfluxPoint(info);
                if (point == null) continue;
                var message = JsonConvert.SerializeObject(point);
                list.Add(message);
            }
            _ = KafkaService.ProduceMessage(list);
        }

        
    }
}
