using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Receive
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost",
            Port = 5672,  UserName = "guest", Password = "guest"};
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange:"Economia", type:ExchangeType.Fanout);
                    channel.ExchangeDeclare(exchange:"Deportes", type:ExchangeType.Fanout);

                    var queuName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue:queuName, exchange:"Economia", routingKey:"");
                    channel.QueueBind(queue:queuName, exchange:"Deportes", routingKey:"");

                    Console.WriteLine("Waiting for logs...");

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"[x] Received {message}");
                    };

                    channel.BasicConsume(queue:queuName, autoAck:true, consumer:consumer);

                    Console.WriteLine("Press any key to exit...");
                    Console.ReadLine();
                }
            }
        }
    }
}