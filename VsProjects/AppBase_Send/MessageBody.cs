using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBase_Send
{
    public class MessageBody
    {
        public string Msg { get; set; }
        public int Year { get; set; }
        public int DayofYear { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public int Second { get; set; }

        public void Refresh()
        {
            var _now = DateTime.Now;
            Year = _now.Year;
            DayofYear = _now.DayOfYear;
            Hour = _now.Hour;
            Minute = _now.Minute;
            Second = _now.Second;
        }

        public MessageBody(String type)
        {
            Msg = type;
            Refresh();
        }
    }
}
