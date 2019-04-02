using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public static class DictionaryExtension
{
    /// <summary>
    /// 根据KEY获取字典值
    /// </summary>
    /// <typeparam name="T">返回值的类型</typeparam>
    /// <param name="dic">扩展类</param>
    /// <param name="key">KEY值</param>
    /// <returns>返回字典值</returns>
    public static T GetValue<T>(this Dictionary<string, T> dic, string key)
    {
        T value;
        if (dic.TryGetValue(key, out value))
            return value;
        else
            return default(T);
    }

    /// <summary>  
    /// 字典类型转化为对象  
    /// </summary>  
    /// <param name="dic"></param>  
    /// <returns></returns>  
    public static T ToObject<T>(this Dictionary<string, object> dic) where T : new()
    {
        var t = new T(); 
        foreach (var d in dic)
        { 
            try
            {
                var value = d.Value;
                t.GetType().GetProperty(d.Key).SetValue(t, value);
            }
            catch (Exception e)
            {

            }
        }
        return t;
    }
}
