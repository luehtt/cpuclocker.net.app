using CPUClocker.Common;
using CPUClocker.Models;
using CPUClocker.Services;
using System;
using System.Collections.Generic;

namespace CPUClocker
{
    class Program
    {
        static async void Main(string[] args)
        {
            Console.WriteLine("Init program");

            try
            {
                // load all config
                await AppConfigService.Load();
                // load current name if default names
                if (AppConfig.ComputerName == "Computer Name") AppConfig.ComputerName = Environment.MachineName;
                if (AppConfig.UserName == "User Name") AppConfig.UserName = Environment.UserName;

                // write monitoring data
                Console.WriteLine($"Monitoring {AppConfig.UserName}'s {AppConfig.ComputerName}...");
                Console.WriteLine($"Sending data each {AppConfig.Interval}ms for {AppConfig.Timeout}min(s)");
                Console.WriteLine($"Using kafka: {AppConfig.UsingKafka}");

                var monitor = new HardwareMonitor(AppConfig.Timeout, AppConfig.Interval, AppConfig.ComputerName, AppConfig.UserName, AppConfig.MonitorHardwares);
                monitor.StartMonitor();
                monitor.LoadKafka();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

            Console.ReadLine();
        }
    }
}
