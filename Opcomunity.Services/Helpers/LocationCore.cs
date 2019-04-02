using log4net;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Helpers
{
    public static class LocationCore
    {
        private static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public static string GetLocationCity()
        {
            try
            {
                var qqApiUrl = ConfigHelper.GetValue("QQLocationApiUrl");
                var qqApiKey = ConfigHelper.GetValue("QQLocationApiKey");
                string url = string.Format("{0}?ip={1}&key={2}", qqApiUrl, WebUtils.GetClientIP(), qqApiKey);
                var data = WebUtils.GetHttpRequestString(url, 8000, 0, "==========");
                if (!string.IsNullOrEmpty(data))
                {
                    JObject json = JObject.Parse(data);
                    if (json["status"].ToString() == "0")
                    {
                        var result = JObject.Parse(json["result"].ToString());
                        var ad_info = JObject.Parse(result["ad_info"].ToString());
                        return ad_info["city"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Error(log,"Message:"+ex.Message+ "    StackTrace:" + ex.StackTrace);
            }
            return string.Empty;
        }
    }
}
