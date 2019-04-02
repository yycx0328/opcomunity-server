using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace WebSite.Test
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/Js")
                .Include("~/scripts/jquery-{version}.js")
                .Include("~/scripts/bootstrap.min.js"));
            
            bundles.Add(new StyleBundle("~/bundles/Css")
                .Include("~/Content/bootstrap.css")
                .Include("~/Content/custom.css"));
        }
    }
}