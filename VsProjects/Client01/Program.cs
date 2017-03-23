using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Client01
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var exchange_name = "ClockTick";


            int rec_count = 0;
            bool isExit = false;
            Console.WriteLine("Client01 正在订阅 SecondTick 消息，按 M 键可退出程序");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange_name, type: "topic");
                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                                 exchange: exchange_name,
                                 routingKey: "#.SecondTick");

                    Task.Factory.StartNew(() =>
                    {
                        while (Console.ReadKey().Key != ConsoleKey.M) ;
                        isExit = true;
                    });

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        //var message = Encoding.UTF8.GetString(body);
                        //var routingKey = ea.RoutingKey;
                        rec_count++;
                        if(rec_count % 2 == 0)
                            Console.Write("\r+   ");
                        else
                            Console.Write("\r-   ");
                    };
                    channel.BasicConsume(queue: queueName,
                                         noAck: true,
                                         consumer: consumer);


                    while (!isExit)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                    channel.BasicCancel(consumer.ConsumerTag);
                }
            }
        }
    }
}
