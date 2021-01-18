using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using CPUClocker.Models;

namespace CPUClocker.Services
{
    class AppConfigService
    {

        public async static Task Load()
        {
            try
            {
                using (var reader = new StreamReader("appConfig.json"))
                {
                    var data = await reader.ReadToEndAsync();
                    var config = JsonConvert.DeserializeObject<AppConfigReader>(data);

                    // load influx config
                    if (!string.IsNullOrEmpty(config.InfluxHost)) AppConfig.InfluxHost = config.InfluxHost;
                    if (!string.IsNullOrEmpty(config.InfluxDb)) AppConfig.InfluxDb = config.InfluxDb;
                    if (!string.IsNullOrEmpty(config.InfluxUser)) AppConfig.InfluxUser = config.InfluxUser;
                    if (!string.IsNullOrEmpty(config.InfluxPassword)) AppConfig.InfluxPassword = config.InfluxPassword;

                    // load kafka config
                    if (!string.IsNullOrEmpty(config.KafkaServer)) AppConfig.KafkaServer = config.KafkaServer;
                    if (!string.IsNullOrEmpty(config.KafkaTopic)) AppConfig.KafkaTopic = config.KafkaTopic;
                    if (!string.IsNullOrEmpty(config.UsingKafka))
                    {
                        if (config.UsingKafka == "1" || config.UsingKafka == "true") AppConfig.UsingKafka = true;
                        if (config.UsingKafka == "0" || config.UsingKafka == "false") AppConfig.UsingKafka = false;
                    }
                    
                    // load app config
                    if (!string.IsNullOrEmpty(config.UsingKafka))
                    {
                        AppConfig.Timeout = int.Parse(config.Timeout);
                    }
                    if (!string.IsNullOrEmpty(config.Interval))
                    {
                        AppConfig.Interval = int.Parse(config.Interval);
                    }
                    if (!string.IsNullOrEmpty(config.ComputerName)) AppConfig.ComputerName = config.ComputerName;
                    if (!string.IsNullOrEmpty(config.UserName)) AppConfig.UserName = config.UserName;
                    if (!string.IsNullOrEmpty(config.MonitorHardwares)) {
                        var monitors = config.MonitorHardwares.Split(' ').ToList();
                        AppConfig.MonitorHardwares = monitors;
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Missing appConfig.json or failed to load. Reading default configuration");
            }
        }
    }
}
