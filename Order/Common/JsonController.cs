using Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Order
{
    public class JsonController : BaseController
    {
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