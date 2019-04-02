using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSite
{
    public class QiniuConfig
    {
        public static string AccessKey = "Bfj3n673J3eLIiB1ExP6dnRmzg1SGWTfmeDpj9iq";
        public static string SecretKey = "4QghaGZstcYx2iWaw6Pa3wDlRUcEjDYAWd2Xr57i";
        public static string Bucket = "isofteam";
        public static string UploadCallbackUrl = "http://api.opcomunity.com/Qiniu/UploadCallback";
        public static int TokenExpiresSecond = 7200;
    }
}