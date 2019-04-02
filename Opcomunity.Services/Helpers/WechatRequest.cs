using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Opcomunity.Services.Helpers
{
    public class WechatRequest
    {
        public WechatRequest(HttpContextBase httpContext)
        {
            parameters = new Hashtable();

            this.httpContext = httpContext;
        }
        
        /** 请求的参数 */
        protected Hashtable parameters;
        protected HttpContextBase httpContext;

        /** 初始化函数。*/
        public virtual void init()
        {
            //nothing to do
        }

        #region 辅助方法===================================
        /// <summary>
        /// 获取paySign 签名
        /// </summary>
        /// <param name="key">key 秘钥的字符名称 就是叫 key</param>
        /// <param name="value">秘钥</param>
        /// <returns></returns>
        public virtual string CreateMd5Sign(string key, string value)
        {
            var sb = new StringBuilder();

            var akeys = new ArrayList(parameters.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (null != v && "".CompareTo(v) != 0 && "sign".CompareTo(k) != 0 && "key".CompareTo(k) != 0)
                {
                    sb.Append(k + "=" + v + "&");
                }
            }

            sb.Append(key + "=" + value);
            string sign = WebUtils.GetMD5(sb.ToString(), getCharset()).ToUpper();
            return sign;
        }

        /// <summary>
        /// 获取预支付 XML 参数组合
        /// </summary>
        /// <returns></returns>
        public string parseXML()
        {
            var sb = new StringBuilder();
            sb.Append("<xml>");
            var akeys = new ArrayList(parameters.Keys);
            foreach (string k in akeys)
            {
                var v = (string)parameters[k];
                if (Regex.IsMatch(v, @"^[0-9.]$"))
                {
                    sb.Append("<" + k + ">" + v + "</" + k + ">");
                }
                else
                {
                    sb.Append("<" + k + "><![CDATA[" + v + "]]></" + k + ">");
                }
            }
            sb.Append("</xml>");
            return sb.ToString();
        }
        #endregion
        
        /** 设置参数值 */
        public void setParameter(string parameter, string parameterValue)
        {
            if (parameter != null && parameter != "")
            {
                if (parameters.Contains(parameter))
                {
                    parameters.Remove(parameter);
                }

                parameters.Add(parameter, parameterValue);
            }
        }
        
        protected virtual string getCharset()
        {
            return this.httpContext.Request.ContentEncoding.BodyName;
        }
    }
}
