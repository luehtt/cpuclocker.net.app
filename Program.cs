using CPUClocker.Common;
using CPUClocker.Models;
using System;
using System.Collections.Generic;

namespace CPUClocker
{
    class Program
    {
        private static int DEFAULT_TIMEOUT = 10000;
        private static int DEFAULT_INTERVAL = 1000;

        static void Main(string[] args)
        {
            Console.WriteLine("Init program");

            try
            {
                var computerName = "Dell B2317H"; // Environment.MachineName;
                var userName = "BSS"; // Environment.UserName;
                var selectedHardware = new List<string> { Const.INFLUX_CPU, Const.INFLUX_RAM, Const.INFLUX_HDD, Const.INFLUX_GPU, Const.INFLUX_MAIN };

                Console.WriteLine($"DEFAULT_TIMEOUT {DEFAULT_TIMEOUT}s");
                Console.WriteLine($"DEFAULT_INTERVAL {DEFAULT_INTERVAL}ms");

                var monitor = new HardwareMonitor(DEFAULT_INTERVAL, DEFAULT_TIMEOUT, computerName, userName, selectedHardware);
                monitor.Start();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            Console.ReadLine();
        }
    }
}
