using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace MailService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                UserName = "guest",
                Password = "guest",
                Port = AmqpTcpEndpoint.UseDefaultPort,
                AutomaticRecoveryEnabled = true
            };
            var endpoints = new List<AmqpTcpEndpoint>
                {
                          new AmqpTcpEndpoint("rabbitmq"),
                          new AmqpTcpEndpoint("localhost")
                };
            using var connection = factory.CreateConnection(endpoints);
            using var channel = connection.CreateModel();
            channel.QueueDeclare("UserService", durable: true, exclusive: false, autoDelete: false, arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(message);
            };
            channel.BasicConsume("UserService", true, consumer);
            Console.ReadLine();
        }
    }
}
