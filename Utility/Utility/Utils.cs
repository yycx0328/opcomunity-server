using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Xsl;

public class Utils
{
    /// <summary>
    /// 判断指定的时间是否为无效（缺省）的时间
    /// </summary>
    /// <param name="dt">要判断的时间值</param>
    /// <returns>是则返回true,否则返回false</returns>
    public static bool IsInvalidDateTime(DateTime dt)
    {
        return (dt.Year <= INVALID_DATETIME.Year);
    }

    /// <summary>
    /// 无效时间(缺省时间)
    /// </summary>
    public static readonly DateTime INVALID_DATETIME = new DateTime(1900, 1, 1);


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