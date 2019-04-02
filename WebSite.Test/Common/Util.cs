using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace WebSite.Test
{
    public class Util
    {
        #region 获取POST参数集合【带签名参数】
        /// <summary>
        /// 获取POST参数集合【带签名参数】
        /// </summary>
        /// <param name="_requestParms">原始参数列表</param>
        /// <param name="signKey">签名秘钥</param>
        /// <returns>返回POST参数集</returns>
        public static NameValueCollection GetPostDataCollection(SortedDictionary<string, string> _requestParms, string signKey)
        {
            NameValueCollection vc = new NameValueCollection();
            string _sign_string = string.Empty;
            if(!_requestParms.ContainsKey("channel"))
                _requestParms.Add("channel", "2000");
            foreach (KeyValuePair<string, string> item in _requestParms)
            {
                _sign_string += string.Format("{0}={1}", item.Key, item.Value);
                vc.Add(item.Key, item.Value);
            }
            string _sign_key = WebUtils.MD5(_sign_string + signKey, "UTF-8").ToLower();
            vc.Add("sign", _sign_key); 
            return vc;
        }
        #endregion
    }
}