using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using BitConvertEx;
using Newtonsoft.Json;

namespace Client02
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            var exchange_name = "ClockTick";
            int minute_msg_count = 0;

            int rec_count = 0;
            bool isExit = false;
            Console.WriteLine("Client01 正在订阅 SecondTick 与 MinuteTick  消息，按 V 键可退出程序");

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: exchange_name, type: "topic");

                    var queueName = channel.QueueDeclare().QueueName;
                    channel.QueueBind(queue: queueName,
                        exchange: exchange_name,
                        routingKey: "#.SecondTick");
                    channel.QueueBind(queue: queueName,
                        exchange: exchange_name,
                        routingKey: "#.MinuteTick");

                    Task.Factory.StartNew(() =>
                    {
                        while (Console.ReadKey().Key != ConsoleKey.V) ;
                        isExit = true;
                    });

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        UInt32 MsgTypeID = body.GetUInt32WithEndianBit(0x0D, BitConverterEx.EndianBitType.BigEndian);

                        if (MsgTypeID == 0x0000002)
                        {
                            Console.Beep();
                            minute_msg_count++;
                        }
                           


                        UInt16 context_len = body.GetUInt16WithEndianBit(0x19, BitConverterEx.EndianBitType.BigEndian);
                        var message = Encoding.UTF8.GetString(body, 32, context_len);
                        var obj = JsonConvert.DeserializeObject<MessageBody>(message);
                        var rec_str = string.Concat(Enumerable.Repeat("*", minute_msg_count % 10)); ;
                        //var routingKey = ea.RoutingKey;

                        rec_count++;
                        if (rec_count % 2 == 0)
                            Console.Write("\r+   " + rec_str);
                        else
                            Console.Write("\r-   " + rec_str);
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
