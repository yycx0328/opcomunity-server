using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order
{
    public class LogConfig
    {
        public static void Register()
        {
            Log4NetHelper.InitLog4net();
        }
    }
}