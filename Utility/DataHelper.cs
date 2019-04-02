/*****************************************************
 * Author: 卢云海
 * Mail: lyh_oralce@hotmail.com
 * QQ: 1289927096
 * Date: 2017-04-20
 * **************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace Common
{
    public class DataHelper
    {
        // js系列化器
        static JavaScriptSerializer jss = new JavaScriptSerializer();

        #region 将Object对象转换成Json字符串
        /// <summary>
        /// 将Object对象转换成Json字符串
        /// </summary>
        /// <param name="obj">要转换的对象</param>
        /// <returns>转换的json字符串</returns>
        public static string ObjectToJson(object obj)
        {
            return jss.Serialize(obj);
        } 
        #endregion

        #region MD5加密
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的32位字符串</returns>
        public static string MD5(string str)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(str, FormsAuthPasswordFormat.MD5.ToString());
        } 
        #endregion
    }
}
