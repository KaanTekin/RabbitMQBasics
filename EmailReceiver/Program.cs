using EmailReceiver.Helpers.Mail;
using EmailReceiver.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace EmailReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
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



                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, eventArgs) =>
                    {
                        var message = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

                        EmailModel emailModel = Newtonsoft.Json.JsonConvert.DeserializeObject<EmailModel>(message);

                        MailHelper mailHelper = new MailHelper(emailModel);
                        mailHelper.SendMail();
                        Console.WriteLine($"Message received... mail sent to {emailModel.To}");

                    };

                    channel.BasicConsume(queue: "email", autoAck: true, consumer: consumer);

                    Console.WriteLine("Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }
    }
}
