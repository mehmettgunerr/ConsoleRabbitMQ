using RabbitMQ.Client;
using System;
using System.Linq;
using System.Text;

namespace ConsoleRabbitMQ.publisher
{
    public enum LogNames
    {
        Critical = 1,
        Error,
        Warning,
        Info
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://uikrjboo:nadE9eNaF-9xWDLBm5KCsTZZ2dcuJV5M@moose.rmq.cloudamqp.com/uikrjboo");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.ExchangeDeclare("logs-topic", durable: true, type: ExchangeType.Topic);

            Random rand = new Random();

            Enumerable.Range(1, 50).ToList().ForEach(x =>
            {
                LogNames log1 = (LogNames)rand.Next(1, 5);
                LogNames log2 = (LogNames)rand.Next(1, 5);
                LogNames log3 = (LogNames)rand.Next(1, 5);

                var routeKey = $"{log1}.{log2}.{log3}";
                string message = $"log-type {log2}-{log2}-{log3}";
                var messageBody = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("logs-topic", routeKey, null, messageBody);

                Console.WriteLine($"Log gönderilmiştir : {message}");
            });

            Console.ReadLine();
        }
    }
}
