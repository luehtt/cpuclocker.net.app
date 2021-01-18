using Confluent.Kafka;
using CPUClocker.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CPUClocker.Services
{
    class KafkaService
    {

        private static ProducerConfig config = new ProducerConfig { BootstrapServers = AppConfig.KafkaServer };

        private static async Task Produce(string message)
        {
            using (var producer = new ProducerBuilder<Ignore, string>(config).Build())
            {
                producer.Produce(AppConfig.KafkaTopic, new Message<Ignore, string> { Value = message });
            }
        }

        private static async Task Consume()
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(AppConfig.KafkaTopic);
            }
        }
    }
}
