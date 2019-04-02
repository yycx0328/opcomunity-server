using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Mvc
{
    public class BaseController : Controller
    { 
        protected internal virtual JsonWebResult ToJson(object data)
        {
            return new JsonWebResult { Data = data, ContentType = "application/json", ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.DenyGet };
        }

        protected internal virtual JsonWebResult ToJsonAllowGet(object data)
        {
            return new JsonWebResult { Data = data, ContentType = "application/json", ContentEncoding = Encoding.UTF8, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        protected internal virtual JsonWebResult ToJson(object data, JsonRequestBehavior behavior)
        {
            return new JsonWebResult { Data = data, ContentType = "application/json", ContentEncoding = Encoding.UTF8, JsonRequestBehavior = behavior };
        }

        protected internal virtual JsonWebResult ToJson(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonWebResult { Data = data, ContentType = contentType, ContentEncoding = contentEncoding, JsonRequestBehavior = behavior };
        }
    }
}