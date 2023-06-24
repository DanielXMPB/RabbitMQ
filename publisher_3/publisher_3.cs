using System;
using System.Text;
using RabbitMQ.Client;

namespace Send
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "192.168.20.27",  UserName = "daniel", Password = "1234"};
            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange:"Politica", type:ExchangeType.Fanout);

                    string message = GetMessage(args);
                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "Politica", routingKey: "", basicProperties: null, body: body);
                    Console.WriteLine($"[x] Send {message}");
                }
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }

        private static string GetMessage(string[] args)
        {
            return ((args.Length > 0)
            ? string.Join("", args)
            : "info: Noticias de Politica");
        }
    }
}