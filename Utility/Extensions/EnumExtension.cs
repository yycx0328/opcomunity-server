using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

#region 枚举扩展类
public static class EnumExtension
{
    #region 获取枚举描述
    /// <summary>
    /// 获取枚举描述
    /// </summary>
    /// <param name="e">扩展类</param>
    /// <returns></returns>
    public static string GetRemark(this Enum e)
    {
        Type type = e.GetType();
        FieldInfo fd = type.GetField(e.ToString());
        if (fd == null)
            return string.Empty;
        object[] attrs = fd.GetCustomAttributes(typeof(RemarkAttribute), false);
        string name = string.Empty;
        foreach (RemarkAttribute attr in attrs)
        {
            name = attr.Remark;
        }
        return name;
    }
    #endregion
} 
#endregion

#region 自定义属性
public class RemarkAttribute : Attribute
{
    // 描述
    private string remark;

    // 构造函数，初始化描述
    public RemarkAttribute(string remark)
    {
        this.remark = remark;
    }

    public string Remark
    {
        get { return remark; }
    }
}
#endregion