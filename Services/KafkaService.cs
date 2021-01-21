using Confluent.Kafka;
using CPUClocker.Models;
using System;
using System.Collections.Generic;
using System.Net;


namespace CPUClocker.Services
{
    class KafkaService
    {

        private static ProducerConfig producerConfig = new ProducerConfig { 
            BootstrapServers = AppConfig.KafkaServer,
            ClientId = Dns.GetHostName()
        };

        private static ConsumerConfig consumerConfig = new ConsumerConfig
        {
            BootstrapServers = AppConfig.KafkaServer,
            GroupId = AppConfig.KafkaTopic,
            ClientId = Dns.GetHostName()
        };

        public static void ProduceMessage(string message)
        {
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                producer.Produce(AppConfig.KafkaTopic, new Message<Null, string> { Value = message });
            }
        }

        public static void ProduceMessage(IEnumerable<string> messages)
        {
            using (var producer = new ProducerBuilder<Null, string>(producerConfig).Build())
            {
                foreach (var message in messages)
                {
                    producer.Produce(AppConfig.KafkaTopic, new Message<Null, string> { Value = message });
                }
                producer.Flush();
            }
        }

        public static void ConsumeMessage()
        {
            try
            {
                using (var consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build())
                {

                    consumer.Subscribe(AppConfig.KafkaTopic);
                    while (true)
                    {
                        var result = consumer.Consume();
                        Console.WriteLine(result.ToString());
                        _ = InfluxService.UploadInflux(result.Message.Value);
                        consumer.Commit();
                    }
                }
            }
            catch (Exception err)
            {
                Console.WriteLine(err.ToString());
            }
        }
    }
}
