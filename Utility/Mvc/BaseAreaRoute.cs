using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc
{
    public class BaseAreaRoute : AreaRegistration
    {
        public override string AreaName
        {
            get { return "Area"; }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                name: this.AreaName+"_default",
                url: this.AreaName + "/{controller}/{action}/{id}",
                defaults: new { id = UrlParameter.Optional},
                namespaces: new[] { "WebSite.Areas."+ this.AreaName +".Controllers" });
        }
    }
}