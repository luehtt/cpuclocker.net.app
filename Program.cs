using CPUClocker.Models;
using CPUClocker.Services;
using System;

namespace CPUClocker
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Init program");

                // load all config
                AppConfigService.Load().Wait();
                // load current name if default names
                if (AppConfig.ComputerName == "Computer Name" || string.IsNullOrEmpty(AppConfig.ComputerName)) AppConfig.ComputerName = Environment.MachineName;
                if (AppConfig.UserName == "User Name" || string.IsNullOrEmpty(AppConfig.UserName)) AppConfig.UserName = Environment.UserName;

                // write monitoring data
                Console.WriteLine($"Monitoring {AppConfig.UserName}'s {AppConfig.ComputerName}...");
                Console.WriteLine($"Sending data each {AppConfig.Interval}ms for {AppConfig.Timeout}min(s)");
                Console.WriteLine($"Using kafka: {AppConfig.UsingKafka}");

                var monitor = new HardwareMonitor(AppConfig.Interval, AppConfig.Timeout, AppConfig.ComputerName, AppConfig.UserName, AppConfig.MonitorHardwares, AppConfig.UsingKafka);
                monitor.StartMonitor();

                if (AppConfig.UsingKafka == true)
                {
                    KafkaService.ConsumeMessage();
                }

                Console.ReadLine();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }

        }
    }
}
