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
using BitConvertEx;

namespace AppBase_Send
{

    class Program
    {
        public static string exchange_name = "ClockTick";
        public static UInt32 AppSourceID = 0x01020304U;

        static void Main(string[] args)
        {
            //BitConverterEx 测试
            //var des = new byte[8];
            //BitConverterEx.ToBytesWithEndianBit((UInt16)76, des,0, BitConverterEx.EndianBitType.LittleEndian);
            //BitConverterEx.ToBytesWithEndianBit((Int16)76, des, 0, BitConverterEx.EndianBitType.LittleEndian);
            //var res = des.GetUInt16WithEndianBit(0, BitConverterEx.EndianBitType.LittleEndian); 
            //System.Diagnostics.Debugger.Break();

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
        private static UInt32 MessageTypeID = 0x00000001U;
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        private MessageBody _msg = new MessageBody("SecondTick");


        public void Execute(IJobExecutionContext context)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var js = JsonConvert.SerializeObject(_msg);
                    //正文填充
                    var s = System.Text.Encoding.UTF8.GetBytes(js, 0, js.Length, second_buf, 32);

                    if (s > (2 *1024 -32))
                        throw new ArgumentOutOfRangeException("正文数据超出范围");

                    //头部填充
                    second_buf[0x00] = 0x01;
                    Program.AppSourceID.ToBytesWithEndianBit(second_buf, 0x01, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt64 utc_now = (UInt64)DateTimeTool.GetTimeUtcstamp(DateTime.Now);
                    utc_now.ToBytesWithEndianBit(second_buf, 0x05, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    MessageTypeID.ToBytesWithEndianBit(second_buf, 0x0D, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt64 expired_time = 0U;
                    expired_time.ToBytesWithEndianBit(second_buf, 0x11, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt16 context_len = (UInt16)s;
                    context_len.ToBytesWithEndianBit(second_buf, 0x19, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);

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
        private static UInt32 MessageTypeID = 0x00000002U;
        private ConnectionFactory factory = new ConnectionFactory() { HostName = "localhost" };
        private MessageBody _msg = new MessageBody("MinuteTick");

        public void Execute(IJobExecutionContext context)
        {
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var js = JsonConvert.SerializeObject(_msg);
                    //正文填充
                    var s = System.Text.Encoding.UTF8.GetBytes(js, 0, js.Length, minute_buf, 32);

                    if (s > (2 * 1024 - 32))
                        throw new ArgumentOutOfRangeException("正文数据超出范围");

                    //头部填充
                    minute_buf[0x00] = 0x01;
                    Program.AppSourceID.ToBytesWithEndianBit(minute_buf, 0x01, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt64 utc_now = (UInt64)DateTimeTool.GetTimeUtcstamp(DateTime.Now);
                    utc_now.ToBytesWithEndianBit(minute_buf, 0x05, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    MessageTypeID.ToBytesWithEndianBit(minute_buf, 0x0D, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt64 expired_time = 0U;
                    expired_time.ToBytesWithEndianBit(minute_buf, 0x11, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);
                    UInt16 context_len = (UInt16)s;
                    context_len.ToBytesWithEndianBit(minute_buf, 0x19, BitConvertEx.BitConverterEx.EndianBitType.BigEndian);

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
