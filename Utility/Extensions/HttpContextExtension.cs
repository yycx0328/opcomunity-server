using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

public static class HttpContextExtension
{
    #region 获取GET和POST参数值（带默认值）
    /// <summary>
    /// 获取GET和POST参数值（带默认值）
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="key">KEY值</param>
    /// <param name="defaultValue">默认值</param>
    /// <returns>返回请求参数值</returns>
    public static string GetRequestValue(this HttpContext context, string key, string defaultValue)
    {
        string value = context.Request.QueryString[key];
        if (string.IsNullOrEmpty(value))
            value = context.Request.Form[key];
        return string.IsNullOrEmpty(value) ? defaultValue : value.Trim();
    }
    #endregion

    #region 获取GET和POST参数值
    /// <summary>
    /// 获取GET和POST参数值
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="key">KEY值</param>
    /// <returns>返回请求参数值</returns>
    public static string GetRequestValue(this HttpContext context, string key)
    {
        return GetRequestValue(context, key, string.Empty);
    }
    #endregion

    #region 获取GET和POST请求参数集合字典
    /// <summary>
    /// 获取GET和POST请求参数集合字典
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <returns>返回GET和POST请求参数集合字典</returns>
    public static Dictionary<string, string> GetRequestParms(this HttpContext context)
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

    #region 获取客户端GET请求的参数
    /// <summary>
    /// 获取客户端GET请求的参数
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="_values">返回拼接字符串</param>
    /// <returns>返回客户端GET请求参数集合字典</returns>
    public static Dictionary<string, string> GetRequestGetParms(this HttpContext context, out string _values)
    {
        Dictionary<string, string> _result = new Dictionary<string, string>();
        _values = string.Empty;
        var _keys = from thiskey in context.Request.QueryString.AllKeys orderby thiskey ascending select thiskey;
        foreach (string _key in _keys)
        {
            if (!_result.ContainsKey(_key))
            {
                _result.Add(_key, context.Request.QueryString[_key]);
                if (!_key.ToLower().Equals("sign")) { _values += context.Request.QueryString[_key]; }
            }
        }
        return _result;
    }
    #endregion

    #region 获取客户端POST请求的参数
    /// <summary>
    /// 获取客户端POST请求的参数
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="_values">返回拼接字符串</param>
    /// <returns>返回客户端POST请求参数集合字典</returns>
    public static Dictionary<string, string> GetRequestPostParms(this HttpContext context, out string _values)
    {
        Dictionary<string, string> _result = new Dictionary<string, string>();
        _values = string.Empty;

        //context.Request.Form.AllKeys[1] = null;
        var _keys = from thiskey in context.Request.Form.AllKeys orderby thiskey ascending select thiskey;
        foreach (string _key in _keys)
        {
            if (_key != null)
            {
                if (!_result.ContainsKey(_key))
                {
                    _result.Add(_key.ToLower(), context.Request.Form[_key]);
                    if (!_key.ToLower().Equals("sign")) { _values += context.Request.Form[_key]; }
                }
            }
            else
            {
                //记录日志
            }
        }
        return _result;
    }
    #endregion

    #region 验证请求参数
    /// <summary>
    /// 验证请求参数
    /// </summary>
    /// <param name="context">扩展类</param>
    /// <param name="_secertKey">加密密钥</param>
    /// <param name="_requestParms">返回请求参数字典</param>
    /// <param name="_state">返回状态描述</param>
    /// <returns>返回验证结果</returns>
    public static bool CheckRequestParam(this HttpContext context, string _secertKey,
    out Dictionary<string, string> _requestParms, out ValidateTips _state)
    {
        string _values = string.Empty;
        _requestParms = GetRequestPostParms(context, out _values);
        bool _result = _requestParms.Count > 0;

        // time和sign是必须参数
        _result = _result && _requestParms.ContainsKey("time") && _requestParms.ContainsKey("sign");
        if (!_result)
        {
            _state = ValidateTips.Error_BaseParams;
            return _result;
        }

        // 验证时间戳格式
        long timeStamp = TypeHelper.TryParse(_requestParms.GetValue("time"), 0L);
        if (timeStamp <= 0)
        {
            _state = ValidateTips.Error_TimeStamp;
            return _result;
        }

        // 验证请求时间，请求时间在30分钟内有效
        DateTime requestTime = TypeHelper.TryParse(_requestParms.GetValue("time"), DateTime.Now.AddYears(-1));
        double totalMinutes = (DateTime.Now - requestTime).TotalMinutes;
        if (totalMinutes > 30 || totalMinutes < 0)
        {
            _state = ValidateTips.Error_Url;
            return _result;
        }

        // 验证签名
        _result = _requestParms.GetValue("sign").ToLower() == WebUtils.MD5(_values + _secertKey, "UTF-8").ToLower();
        if (!_result)
        {
            _state = ValidateTips.Error_Sign;
            return _result;
        }
        _state = ValidateTips.Success;
        return _result;
    } 
    #endregion
}
