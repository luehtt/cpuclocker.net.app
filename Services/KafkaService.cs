using Confluent.Kafka;
using CPUClocker.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CPUClocker.Services
{
    class KafkaService
    {

        private static ProducerConfig config = new ProducerConfig { 
            BootstrapServers = AppConfig.KafkaServer
        };

        public static void ProduceMessage(string message)
        {
            using (var producer = new ProducerBuilder<Ignore, string>(config).Build())
            {
                producer.Produce(AppConfig.KafkaTopic, new Message<Ignore, string> { Value = message });
            }
        }

        public static void ProduceMessage(IEnumerable<string> messages)
        {
            using (var producer = new ProducerBuilder<Ignore, string>(config).Build())
            {
                foreach (var message in messages)
                {
                    producer.Produce(AppConfig.KafkaTopic, new Message<Ignore, string> { Value = message });
                }
            }
        }

        public static void ConsumeMessage()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(AppConfig.KafkaTopic);

                while (true)
                {
                    var result = consumer.Consume();
                    _ = InfluxService.UploadInflux(result.Message.Value);
                    consumer.Commit();
                }
            }
        }
    }
}
