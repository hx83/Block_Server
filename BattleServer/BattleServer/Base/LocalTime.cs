using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Utils
{
    public class LocalTime
    {
        public static double NowSeconds
        {
            get
            {
                TimeSpan span = DateTime.Now - DateTime.Parse("1970-1-1");
                return span.TotalSeconds;
            }
        }

        public static double NowMilliseconds
        {
            get
            {
                TimeSpan span = DateTime.Now - DateTime.Parse("1970-1-1");
                return span.TotalMilliseconds;
            }
        }
    }
}
