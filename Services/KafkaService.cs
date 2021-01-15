using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CPUClocker.Services
{
    class KafkaService
    {
        private static string host = "http://localhost:9092";
        private static string topic = "cpuclocker";

        private static ProducerConfig config = new ProducerConfig { BootstrapServers = host };

        private static async Task Produce(string topic)
        {
            using (var consumer = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumer.Subscribe(topic);
            }
        }
    }
}
