using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

public class JSONSerializeUtil
{
    #region 将对象转换成Json
    /// <summary>
    /// 将对象转换成Json
    /// </summary>
    /// <typeparam name="T">要转换的类型</typeparam>
    /// <param name="t">要转换的对象</param>
    /// <returns>返回json字符串</returns>
    public static string ToJson<T>(T t)
    {
        string strJson = ToJson(t, null);
        string p = @"\\/Date\((\d+)\)\\/";
        MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
        Regex reg = new Regex(p);
        strJson = reg.Replace(strJson, matchEvaluator);
        return strJson;
    }

    private static string ConvertJsonDateToDateString(Match m)
    {
        string result = m.Groups[1].Value;
        return result;
    }
    #endregion

    #region 将对象转换成Json
    /// <summary>
    /// 将对象转换成Json
    /// </summary>
    /// <typeparam name="T">要转换的类型</typeparam>
    /// <param name="t">要转换的对象</param>
    /// <param name="jsonConverters">Json转换器</param>
    /// <returns>返回json字符串</returns>
    public static string ToJson<T>(T t, IEnumerable<JavaScriptConverter> jsonConverters)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        if (jsonConverters != null) serializer.RegisterConverters(jsonConverters ?? new JavaScriptConverter[0]);
        return serializer.Serialize(t);
    }
    #endregion

    #region 将Json转换成对象
    /// <summary>
    /// 将Json转换成对象
    /// </summary>
    /// <typeparam name="TEntity">要转换的对象类型</typeparam>
    /// <param name="json">要转换的json字符串</param>
    /// <returns>返回转换后的对象</returns>
    public static TEntity ToObject<TEntity>(string json)
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();
        return serializer.Deserialize<TEntity>(json);
    } 
    #endregion
}