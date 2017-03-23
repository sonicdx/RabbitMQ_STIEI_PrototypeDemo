using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        public void Execute(IJobExecutionContext context)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: Program.exchange_name, type: "topic");
                    channel.BasicPublish(exchange: Program.exchange_name,
                        routingKey: "01020304.SecondTick",
                        basicProperties: null,
                        body: second_buf);
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
