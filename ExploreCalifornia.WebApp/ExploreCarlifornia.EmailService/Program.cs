using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace ExploreCalifornia.EmailService
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp:guest:guest@locahhost:5672");

            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            //Declare queue

            channel.QueueDeclare("emailQueue", true, false, false);
            //Bind queue to exchange
            channel.QueueBind("emailQueue", "myExchange ", "");
            //create a consumer
            var consumer = new EventingBasicConsumer(channel);
            //fires a receive message

            consumer.Received += (sender, eventArgs) =>
            {
                //decode
                var msg = System.Text.Encoding.UTF8.GetString(eventArgs.Body.ToArray());
                 
                Console.WriteLine(msg); 
            };
            channel.BasicConsume("emailQueue",true,consumer);
            Console.ReadLine(); //keeps application from closing
            channel.Close();
            connection.Close();
        }
        
    }
}
  
