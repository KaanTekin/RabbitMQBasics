using EmailSender.Models;
using RabbitMQ.Client;
using System;
using System.Text;

namespace EmailSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("From: ");
            string from = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            Console.WriteLine("To Addresses (seperate with ;):");
            string to = Console.ReadLine();
            Console.WriteLine("Subject");
            string subject = Console.ReadLine();
            Console.WriteLine("Body");
            string body = Console.ReadLine();
            Console.WriteLine("Port");
            int port = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Host:");
            string host = Console.ReadLine();


            var connFactory = new ConnectionFactory { HostName = "localhost" };

            using (var connection = connFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "email",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                        );

                    EmailModel emailModel = new EmailModel
                    {
                        Body = body,
                        EnableSsl = true,
                        From = from,
                        Host = host,
                        IsBodyHtml = true,
                        Password = password,
                        Port = port,
                        Subject = subject,
                        To = to,
                        UseDefaultCredentials = false
                    };

                    string serialized = Newtonsoft.Json.JsonConvert.SerializeObject(emailModel);

                    var messageBody = Encoding.UTF8.GetBytes(serialized);

                    channel.BasicPublish(exchange: "",
                        routingKey: "email",
                        basicProperties: null,
                        body: messageBody);

                    Console.WriteLine($"mail sent to {to}");
                }
            }
        }
    }
}
