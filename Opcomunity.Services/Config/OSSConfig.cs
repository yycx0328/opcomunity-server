using Opcomunity.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Opcomunity.Services
{
    public class OSSConfig
    { 
        public static string AccessKeyId
        {
            get { return ConfigHelper.GetValue("OSSAccessKeyId"); }
        }
        public static string AccessKeySecret
        {
            get { return ConfigHelper.GetValue("OSSAccessKeySecret"); }
        }
        public static string Host
        {
            get { return ConfigHelper.GetValue("OSSHost"); }
        }
        public static string Endpoint
        {
            get { return string.Format("http://{0}", ConfigHelper.GetValue("OSSHost"));  }
        }
        public static string ImageBucketPrefix
        {
            get { return ConfigHelper.GetValue("OSSImageBucketPrefix"); }
        }
        public static string VideoBucketPrefix
        {
            get { return ConfigHelper.GetValue("OSSVideoBucketPrefix"); }
        }
    }
}