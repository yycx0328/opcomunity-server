using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Mvc
{
    public class JsonWebResult: JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if ((this.JsonRequestBehavior == JsonRequestBehavior.DenyGet) && string.Equals(context.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidOperationException();
            }
            HttpResponseBase response = context.HttpContext.Response;
            if (!string.IsNullOrEmpty(this.ContentType))
            {
                response.ContentType = this.ContentType;
            }
            else
            {
                response.ContentType = "application/json";
            }
            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = this.ContentEncoding;
            }
            if (this.Data != null)
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                string jsonString = jss.Serialize(Data);

                jsonString = Regex.Replace(jsonString, @"\\/Date\((\d+)\)\\/", match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                });

                //string p = @"\\/Date\((\d+)\)\\/";
                //MatchEvaluator matchEvaluator = new MatchEvaluator(this.ConvertJsonDateToDateString);
                //Regex reg = new Regex(p);
                //jsonString = reg.Replace(jsonString, matchEvaluator);

                var jsoncallback = context.RequestContext.HttpContext.Request.QueryString["jsoncallback"];
                if (string.IsNullOrEmpty(jsoncallback)) response.Write(jsonString);
                else response.Write(jsoncallback + "(" + jsonString + ")");
            }
        }

        private string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            result = m.Groups[1].Value;
            return result;
        }
    }
}