using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBase_Send
{
    public class DateTimeTool
    {
        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数
        /// </summary>
        public static long GetTimestamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.Ticks - dt1970.Ticks) / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// 获取1970-01-01至dateTime的毫秒数,UTC时间
        /// </summary>
        public static long GetTimeUtcstamp(DateTime dateTime)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return (dateTime.ToUniversalTime().Ticks - dt1970.Ticks) / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// 根据时间戳timestamp（单位毫秒）计算日期
        /// </summary>
        public static DateTime NewDate(long timestamp)
        {
            DateTime dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long t = dt1970.Ticks + timestamp * TimeSpan.TicksPerMillisecond;
            return new DateTime(t);
        }
    }
}
