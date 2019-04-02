using log4net;
using Mvc;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Configuration;
using System;
using Opcomunity.Services.Helpers;

namespace WebSite
{
    public abstract class JsonController : BaseController
    {
        protected static readonly string SECURITYKEY = ConfigHelper.GetValue("ApiSecurityKey");

        protected internal virtual JsonWebResult Try(Func<JsonWebResult> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                JsonBase json = new JsonBase();
                json.state = (int)ValidateTips.Error_Exception;
                json.message = ValidateTips.Error_Exception.GetRemark();
                ExceptionLogHelper.Instance.WriteExceptionLog(ex);
                return ToJson(json);
            }
        }
    }
}