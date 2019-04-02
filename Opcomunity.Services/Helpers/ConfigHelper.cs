using Infrastructure;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Helpers
{
    public static class ConfigHelper
    {
        private static readonly IConfigService service = Ioc.Get<IConfigService>();
        public static string GetValue(string key)
        {
            return service.GetValue(key);
        }
        public static int GetValue(string key, int defaultValue)
        {
            var value = service.GetValue(key);
            if(!string.IsNullOrEmpty(value))
                return int.Parse(value);
            return defaultValue;
        }
    }
}
