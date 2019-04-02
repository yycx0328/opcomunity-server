using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public static class WebExtensions
{
    #region 根据Key获取GET/POST请求参数
    /// <summary>
    /// 根据Key获取GET/POST请求参数，并指定默认值
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="key">键</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns></returns>
    public static string GetRequestValue(this HttpContext context, string key, string defaultValue)
    {
        string value = context.Request.QueryString[key];
        if (string.IsNullOrEmpty(value))
            value = context.Request.Form[key];
        return string.IsNullOrEmpty(value) ? defaultValue : value.Trim();
    }

    /// <summary>
    /// 根据Key获取GET/POST请求参数，并指定默认值为空
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="key">键</param>
    /// <returns></returns>
    public static string GetRequestValue(this HttpContext context, string key)
    {
        return GetRequestValue(context, key, string.Empty);
    }
    #endregion

    #region 获取客户端GET/POST请求所有参数字典集合（不作排序处理）
    /// <summary>
    /// 获取所有GET/POST请求参数字典集合（不作排序处理）
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <returns></returns>
    public static Dictionary<string, string> GetRequestParms(this HttpContextBase context)
    {
        Dictionary<string, string> _result = new Dictionary<string, string>();
        if (context.Request.HttpMethod.ToUpper() == "GET")
        {
            var _keys = from thiskey in context.Request.QueryString.AllKeys orderby thiskey ascending select thiskey;
            foreach (string _key in _keys)
            {
                if (!_result.ContainsKey(_key)) { _result.Add(_key, context.Request.QueryString[_key]); }
            }
        }
        else if (context.Request.HttpMethod.ToUpper() == "POST")
        {
            var _keys = from thiskey in context.Request.Form.AllKeys orderby thiskey ascending select thiskey;
            foreach (string _key in _keys)
            {
                if (!_result.ContainsKey(_key)) { _result.Add(_key, context.Request.Form[_key]); }
            }
        }
        return _result;
    }
    #endregion

    //#region 验证POST请求参数集合，并返回验证提示
    ///// <summary>
    ///// 验证POST请求参数集合，并返回验证提示
    ///// </summary>
    ///// <param name="context">扩展类</param>
    ///// <param name="_secertKey">秘钥</param>
    ///// <param name="_requestParms">返回请求参数字典集合</param>
    ///// <param name="_state">验证提示</param>
    ///// <returns></returns>
    //public static bool CheckPostRequestParam(this HttpContextBase context, string _secertKey,
    //    out Dictionary<string, string> _requestParms, out ValidateTips _state)
    //{
    //    string _values = string.Empty;
    //    _requestParms = GetRequestPostParms(context, out _values);
    //    bool _result = _requestParms.Count > 0; 
    //    _result = _result && _requestParms.ContainsKey("timespan") && _requestParms.ContainsKey("sign");
    //    if (!_result)
    //    {
    //        _state = ValidateTips.Error_BaseParams;
    //        return _result;
    //    }
    //    _result = _requestParms["sign"].ToLower() == WebUtils.MD5(_values + _secertKey, "UTF-8").ToLower();
    //    if (!_result)
    //    {
    //        _state = ValidateTips.Error_Sign;
    //        return _result;
    //    }
    //    _state = ValidateTips.Success;
    //    return _result;
    //}

    ///// <summary>
    ///// 获取客户端POST请求的参数
    ///// </summary>
    ///// <param name="context">扩展类</param>
    ///// <param name="_values">拼接请求字符串，格式：key1=valuekey2=value2</param>
    ///// <returns></returns>
    //private static Dictionary<string, string> GetRequestPostParms(this HttpContextBase context, out string _values)
    //{
    //    Dictionary<string, string> _result = new Dictionary<string, string>();
    //    _values = string.Empty;

    //    var _keys = from thiskey in context.Request.Form.AllKeys orderby thiskey ascending select thiskey;
    //    foreach (string _key in _keys)
    //    {
    //        if (!string.IsNullOrEmpty(_key) && !_result.ContainsKey(_key))
    //        {
    //            _result.Add(_key.ToLower(), context.Request.Form[_key]);
    //            if (!_key.ToLower().Equals("sign")) { _values += string.Format("{0}={1}", _key, context.Request.Form[_key]); }
    //        }
    //    }
    //    return _result;
    //}
    //#endregion 
}