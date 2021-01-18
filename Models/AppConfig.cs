using CPUClocker.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPUClocker.Models
{
    class AppConfig
    {
        public static string InfluxHost = "http://localhost:8086";
        public static string InfluxDb = "cpuclocker";
        public static string InfluxUser = "influxdb";
        public static string InfluxPassword = "password";

        public static string KafkaServer = "http://localhost:9092";
        public static string KafkaTopic = "cpuclocker";
        public static bool UsingKafka = false;

        public static int Timeout = 60; // 60m = 1hr
        public static int Interval = 1000; // 1s
        public static string ComputerName = "Computer Name";
        public static string UserName = "User Name";
        public static List<string> MonitorHardwares = new List<string> { Const.INFLUX_CPU, Const.INFLUX_RAM, Const.INFLUX_HDD, Const.INFLUX_GPU, Const.INFLUX_MAIN };
    }

    class AppConfigReader
    {
        public string InfluxHost { get; set; }
        public string InfluxDb { get; set; }
        public string InfluxUser { get; set; }
        public string InfluxPassword { get; set; }

        public string KafkaServer { get; set; }
        public string KafkaTopic { get; set; }
        public string UsingKafka { get; set; }

        public string Timeout { get; set; }
        public string Interval { get; set; }
        public string ComputerName { get; set; }
        public string UserName { get; set; }
        public string MonitorHardwares { get; set; }
    }
}
