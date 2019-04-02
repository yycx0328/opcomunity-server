using Jiguang.JSMS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Helpers
{ 
    public class JPushCore
    {
        public static string SendCode(string phoneNo, int tempplateId)
        {
            JSMSClient client = new JSMSClient(ConfigHelper.GetValue("JPushAppKey"), ConfigHelper.GetValue("JPushMasterSecret"));
            var resp = client.SendCode(phoneNo, tempplateId);
            return resp.Content;
        }
         
        public static string ValidateCode(string messageId, string code)
        {
            JSMSClient client = new JSMSClient(ConfigHelper.GetValue("JPushAppKey"), ConfigHelper.GetValue("JPushMasterSecret"));
            var resp = client.IsCodeValid(messageId, code);
            return resp.Content;
        }
    }
}
