using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace ConsoleRabbitMQ.subscriber
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqps://uikrjboo:nadE9eNaF-9xWDLBm5KCsTZZ2dcuJV5M@moose.rmq.cloudamqp.com/uikrjboo");

            using var connection = factory.CreateConnection();

            var channel = connection.CreateModel();

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            var queueName = "direct-queue-Critical";

            channel.BasicConsume(queueName, false, consumer);

            Console.WriteLine("Loglar dinleniyor..");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);

                Console.WriteLine("Gelen mesaj :" + message);

                //File.AppendAllText("log-critical.txt", message + "\n");

                channel.BasicAck(e.DeliveryTag, false);
            };

            Console.ReadLine();
        }
    }
}
