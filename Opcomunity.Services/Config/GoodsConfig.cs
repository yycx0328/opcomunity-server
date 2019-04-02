using Opcomunity.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opcomunity.Services
{
    public class GoodsConfig
    { 
        public static string BODY
        {
            get { return ConfigHelper.GetValue("GoodsBody"); }
        }
        public static string SUBJECT
        {
            get { return ConfigHelper.GetValue("GoodsSubject"); }
        }
        public static int ExchargeRate
        {
            get { return ConfigHelper.GetValue("GoodsExchargeRate",10); }
        }
    }
}