using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Opcomunity.Services.Helpers
{ 
    public class WechatResponse
    {
        /** 密钥 */
        private string key;

        /** 应答的参数 */
        private Hashtable xmlMap;

        /** debug信息 */
        private string debugInfo;

        protected HttpContextBase httpContext;

        //获取服务器通知数据方式，进行参数获取
        public WechatResponse(HttpContextBase httpContext)
        {
            xmlMap = new Hashtable();

            this.httpContext = httpContext;
            if (this.httpContext.Request.InputStream.Length > 0)
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(this.httpContext.Request.InputStream);
                XmlNode root = xmlDoc.SelectSingleNode("xml");
                XmlNodeList xnl = root.ChildNodes;

                foreach (XmlNode xnf in xnl)
                {
                    xmlMap.Add(xnf.Name, xnf.InnerText);
                }
            } 
        }

        /** 获取密钥 */
        public string getKey()
        { return key; }

        /** 设置密钥 */
        public void setKey(string key)
        { this.key = key; }
         
        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string getParameter(string parameter)
        {
            var s = (string)xmlMap[parameter];
            return (null == s) ? "" : s;
        }
         
        protected virtual string getCharset()
        {
            return this.httpContext.Response.ContentEncoding.BodyName; 
        }
         
        #region 辅助方法===================================
        /// <summary>
        /// 判断微信签名
        /// </summary>
        /// <returns></returns>
        public virtual bool isWXsign()
        {
            StringBuilder sb = new StringBuilder();
            Hashtable signMap = new Hashtable();
            foreach (string k in xmlMap.Keys)
            {
                if (k != "sign")
                {
                    signMap.Add(k.ToLower(), xmlMap[k]);
                }
            }

            ArrayList akeys = new ArrayList(signMap.Keys);
            akeys.Sort();

            foreach (string k in akeys)
            {
                string v = (string)signMap[k];
                sb.Append(k + "=" + v + "&");
            }
            sb.Append("key=" + this.key);

            string sign = WebUtils.GetMD5(sb.ToString(), getCharset()).ToString().ToUpper();
            return sign.Equals(xmlMap["sign"]);

        }
        #endregion
    }
}
