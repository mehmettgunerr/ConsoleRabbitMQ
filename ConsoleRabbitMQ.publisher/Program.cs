using RabbitMQ.Client;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ConsoleRabbitMQ.publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://uikrjboo:nadE9eNaF-9xWDLBm5KCsTZZ2dcuJV5M@moose.rmq.cloudamqp.com/uikrjboo");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("header-exchange", durable: true, type: ExchangeType.Headers);

            Dictionary<string, object> headers = new Dictionary<string, object>();

            headers.Add("format", "pdf");
            //headers.Add("shape","a4");
            headers.Add("shape2", "a4");

            var properties = channel.CreateBasicProperties();
            properties.Headers = headers;
            properties.Persistent = true;

            var product = new Product
            {
                Id = 1,
                Name = "Kalem",
                Price = 100,
                Stock = 10
            };

            var productJsonString = JsonSerializer.Serialize(product);

            channel.BasicPublish("header-exchange", string.Empty, properties, Encoding.UTF8.GetBytes(productJsonString));

            Console.WriteLine("mesaj gönderilmiştir.");

            Console.ReadLine();
        }
    }
}
