﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
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

            var randomQueueName = channel.QueueDeclare().QueueName;

            //channel.QueueDeclare("sabit-queue", true, false, false); -> kuyruk sabit oluşurdu 

            channel.QueueBind(randomQueueName, "logs-fanout", "", null); // uygulama down olduğunda kuyrukta silinir.
            //eğer kuyruk declare etseydik uygulama kapanırsa kuyruk yinede kalırdı

            //channel.QueueDeclare("hello-queue", true, false, false);

            channel.BasicQos(0, 1, false); //sınırsız boyut, aynı anda 5 adet mesaj, eğer false olursa her consumer'a 5 adet yollar , true olursa 5 adet mesajı dinleyenlere bölerek max 5 adet mesaj dağıtılcak gibi davranır.

            var consumer = new EventingBasicConsumer(channel);

            // channel.BasicConsume("hello-queue", true, consumer); ->mesaj işlenince direkt silinir.

            channel.BasicConsume(randomQueueName, false, consumer);

            Console.WriteLine("Loglar dinleniyor..");

            consumer.Received += (object sender, BasicDeliverEventArgs e) =>
            {
                var message = Encoding.UTF8.GetString(e.Body.ToArray());

                Thread.Sleep(1500);

                Console.WriteLine("Gelen mesaj :" + message);

                channel.BasicAck(e.DeliveryTag, false); /* kuyruktan sil */
            };

            Console.ReadLine();
        }
    }
}
