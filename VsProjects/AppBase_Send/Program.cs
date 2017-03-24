using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BitConvertEx;
using Newtonsoft.Json;
using Quartz;
using Quartz.Impl;
using RabbitMQ.Client;

namespace AppBase_Send
{

    class Program
    {
        public  static string exchange_name = "ClockTick";
        static void Main(string[] args)
        {
            //BitConverterEx 测试
            var des = new byte[8];
            //BitConverterEx.ToBytesWithEndianBit((UInt16)76, des,0, BitConverterEx.EndianBitType.LittleEndian);
            BitConverterEx.ToBytesWithEndianBit((Int16)76, des, 0, BitConverterEx.EndianBitType.LittleEndian);
            var res = des.GetUInt16WithEndianBit(0, BitConverterEx.EndianBitType.LittleEndian); 
            System.Diagnostics.Debugger.Break();

            var factory = new ConnectionFactory() { HostName = "localhost" };
            bool isExit = false;
            Console.WriteLine("正在向队列发送消息，按 X 键可退出程序");
            Task.Factory.StartNew(() =>
            {
                while (Console.ReadKey().Key != ConsoleKey.X) ;
                isExit = true;
            });

            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();
            IJobDetail sec_job = JobBuilder.Create<SecondTickMessageSendJob>().Build();
            ITrigger sec_trigger = TriggerBuilder.Create()
                .StartNow()
                .WithDailyTimeIntervalSchedule(
                s => s.WithIntervalInSeconds(1))
                .Build();
            scheduler.ScheduleJob(sec_job, sec_trigger);
            IJobDetail min_job = JobBuilder.Create<MinuteTickMessageSendJob>().Build();
            ITrigger min_trigger = TriggerBuilder.Create()
                .StartNow()
                .WithDailyTimeIntervalSchedule(
                s => s.WithIntervalInMinutes(1))
                .Build();
            scheduler.ScheduleJob(min_job, min_trigger);



            while (!isExit)
            {
                System.Threading.Thread.Sleep(200);
            }
            scheduler.Shutdown();

        }
    }

    class ConsoleWriteJob:IJob
    {

        public void Execute(IJobExecutionContext context)
        {
            Console.WriteLine("x");
        }
    }

    class SecondTickMessageSendJob : IJob
    {
        private byte[] second_buf = new byte[2 * 1024];
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        private MessageBody _msg = new MessageBody("SecondTick");
        public void Execute(IJobExecutionContext context)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var js = JsonConvert.SerializeObject(_msg);
                    var s = System.Text.Encoding.UTF8.GetBytes(js, 0, js.Count(), second_buf, 32);


                    //头部填充
                    second_buf[0x00] = 0x01;
                    second_buf[0x01] = 0x01; second_buf[0x02] = 0x02; second_buf[0x03] = 0x03; second_buf[0x04] = 0x04;

                    channel.ExchangeDeclare(exchange: Program.exchange_name, type: "topic");
                    channel.BasicPublish(exchange: Program.exchange_name,
                        routingKey: "01020304.SecondTick",
                        basicProperties: null,
                        body: second_buf);

                    js = null;
                }
            }

        }
    }

    class MinuteTickMessageSendJob : IJob
    {
        private byte[] minute_buf = new byte[2 * 1024];
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        public void Execute(IJobExecutionContext context)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: Program.exchange_name, type: "topic");
                    channel.BasicPublish(exchange: Program.exchange_name,
                        routingKey: "01020304.MinuteTick",
                        basicProperties: null,
                        body: minute_buf
                        );
                }
            }

        }
    }
}
