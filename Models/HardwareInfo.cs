using CPUClocker.Common;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Text;

namespace CPUClocker.Models
{
    public class HardwareInfo
    {
        public ISensor[] Sensors { get; set; }
        public string MachineName { get; set; }
        public string UserName { get; set; }
        public string HardwareName { get; set; }
        public HardwareType HardwareType { get; set; }
        public Identifier Identifier { get; set; }

        public HardwareInfo(string machineName, string userName, IHardware hardware)
        {
            MachineName = machineName;
            UserName = userName;
            HardwareName = hardware.Name;
            HardwareType = hardware.HardwareType;
            Sensors = hardware.Sensors;
            Identifier = hardware.Identifier;
        }

        public static string GetSensorName(SensorType type)
        {
            switch (type)
            {
                case SensorType.Voltage:
                    return "Voltage";
                case SensorType.Clock:
                    return "Clock";
                case SensorType.Temperature:
                    return "Temperature";
                case SensorType.Load:
                    return "Load";
                case SensorType.Fan:
                    return "Fan";
                case SensorType.Flow:
                    return "Flow";
                case SensorType.Control:
                    return "Control";
                case SensorType.Level:
                    return "Level";
                case SensorType.Factor:
                    return "Factor";
                case SensorType.Power:
                    return "Power";
                case SensorType.Data:
                    return "Data";
                default:
                    return "Undefined";
            }
        }

        public static string GetHardwareTypeName(HardwareType type)
        {
            switch (type)
            {
                case HardwareType.CPU:
                    return Const.INFLUX_CPU;
                case HardwareType.GpuAti:
                    return Const.INFLUX_GPU;
                case HardwareType.GpuNvidia:
                    return Const.INFLUX_GPU;
                case HardwareType.HDD:
                    return Const.INFLUX_HDD;
                case HardwareType.RAM:
                    return Const.INFLUX_RAM;
                case HardwareType.Mainboard:
                    return Const.INFLUX_MAIN;
                default:
                    return "Other";
            }
        }

    }
}
