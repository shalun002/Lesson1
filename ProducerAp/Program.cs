using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProducerAp
{
    class Program
    {
        public static string GeneratePin()
        {
            string pin = string.Empty;
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                pin += random.Next(0, 40);
            }
            return pin;
        }

        public static void ProduceMessageToQueue(string message, string queueName)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (IConnection connection = factory.CreateConnection())
            using (IModel channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: queueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                    routingKey: queueName,
                    basicProperties: null,
                    body: body);

                Console.WriteLine(" [x] Sent {0}", message);
                Console.ReadLine();
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                ProduceMessageToQueue("Alfar is a good boy!", "messages");
                Thread.Sleep(500);
            }
        }
    }
}