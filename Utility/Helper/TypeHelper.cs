using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Summary description for TypeHelper.
/// </summary>
public static class TypeHelper
{
    private static string ObjectAssemblyFullName = typeof(object).Assembly.FullName;
    private static Dictionary<string, Type> TypeMappings = new Dictionary<string, Type>();

    #region TryParse
    /// <summary>
    /// string ==&gt; short
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static short TryParse(string inputText, short defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        short result;
        return short.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; int
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static int TryParse(string inputText, int defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        int result;
        return int.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; long
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static long TryParse(string inputText, long defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        long result;
        return long.TryParse(inputText, out result) ? result : defaultValue;
    }

    public static decimal TryParse(string inputText, decimal defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        decimal result;
        return decimal.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; bool
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
    /// <returns></returns>
    public static bool TryParse(string inputText, bool defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        bool result;
        return bool.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; DateTime
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static DateTime TryParse(string inputText, DateTime defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        DateTime result;
        return DateTime.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; float
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static double TryParse(string inputText, double defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        double result;
        return double.TryParse(inputText, out result) ? result : defaultValue;
    }

    /// <summary>
    /// string ==&gt; int[]
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static int[] TryParse(string inputText, int[] defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        try
        {
            return inputText.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(p => int.Parse(p)).ToArray();
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// string ==&gt; Guid
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static Guid TryParse(string inputText, Guid defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        try
        {
            return new Guid(inputText);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// string ==&gt; Enum
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static Enum TryParse(string inputText, Enum defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        try
        {
            return (Enum)Enum.Parse(defaultValue.GetType(), inputText, true);
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// string ==&gt; string
    /// </summary>
    /// <param name="inputText">The input text.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static string TryParse(string inputText, string defaultValue)
    {
        if (string.IsNullOrEmpty(inputText))
            return defaultValue;
        else
            return inputText;
    }

    /// <summary>
    /// TryParse指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <param name="defaultValue"></param>
    /// <returns></returns>
    public static T TryParse<T>(Func<T> func, T defaultValue)
    {
        try
        {
            return func();
        }
        catch
        {
            return defaultValue;
        }
    }

    /// <summary>
    /// TryParse指定类型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    public static T TryParse<T>(Func<T> func)
    {
        return TryParse<T>(func, default(T));
    }
    #endregion

    /// <summary>
    /// 扩展忽略大小写的Equals
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static bool IEquals(this string str1, string str2)
    {
        return string.Equals(str1, str2, StringComparison.InvariantCultureIgnoreCase);
    }

    /// <summary>
    /// 扩展忽略大小写的Contains
    /// </summary>
    /// <param name="str1"></param>
    /// <param name="str2"></param>
    /// <returns></returns>
    public static bool IContains(this string str1, string str2)
    {
        if (string.IsNullOrEmpty(str1))
            return string.IsNullOrEmpty(str2);

        return (str1.IndexOf(str2, StringComparison.InvariantCultureIgnoreCase) >= 0);
    }

    /// <summary>
    /// GetSubString
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="maxLength">Length of the max.</param>
    /// <returns></returns>
    public static string GetSubString(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;
        else if (value.Length > maxLength)
            return value.Substring(0, maxLength);
        else
            return value;
    }

    /// <summary>
    /// try to return a appropriate type
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="defaultType">The default type.</param>
    /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
    /// <returns></returns>
    public static Type TryGetType(string typeName, Type defaultType, bool ignoreCase)
    {
        Type ret = null;
        if (!string.IsNullOrEmpty(typeName))
        {
            string typeKey = typeName;
            if (TypeMappings.TryGetValue(typeKey, out ret))
                return ret ?? defaultType;

            ret = Type.GetType(typeName, false, ignoreCase);
            if (ret == null && typeName.IndexOf(',') == -1 && typeName.IndexOf('.') > 0)
            {
                typeKey = typeName + ", " + ObjectAssemblyFullName;
                if (TypeMappings.TryGetValue(typeKey, out ret))
                    return ret ?? defaultType;

                ret = Type.GetType(typeKey, false, ignoreCase);
                if (ret == null)
                {
                    typeKey = typeName + ", " + typeName.Substring(0, typeName.LastIndexOf('.'));
                    if (TypeMappings.TryGetValue(typeKey, out ret))
                        return ret ?? defaultType;

                    ret = Type.GetType(typeKey, false, ignoreCase);
                }
            }
            TypeMappings[typeName] = ret;
        }

        return ret ?? defaultType;
    }

    /// <summary>
    /// try to return a appropriate type
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <param name="defaultType">The default type.</param>
    /// <returns></returns>
    public static Type TryGetType(string typeName, Type defaultType)
    {
        return TryGetType(typeName, defaultType, false);
    }

    /// <summary>
    /// Tries the type of the get.
    /// </summary>
    /// <param name="typeName">Name of the type.</param>
    /// <returns></returns>
    public static Type TryGetType(string typeName)
    {
        return TryGetType(typeName, null, false);
    }

    /// <summary>
    /// AddTypeMapping
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public static void AddTypeMapping(string name, Type type)
    {
        TypeMappings[name] = type;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static string GetShortTypeName(Type type)
    {
        if (type.FullName == "System.RuntimeType" && type.BaseType != null)
            type = type.BaseType;

        if (type == null)
            return null;

        string fullName = type.FullName.Replace(", " + ObjectAssemblyFullName, string.Empty);
        if (type == TryGetType(fullName))
            return fullName;

        if (type == TryGetType(fullName + ", " + ObjectAssemblyFullName))
            return fullName;

        fullName = type.FullName + ", " + type.Assembly.FullName.Split(',')[0];
        if (type == TryGetType(fullName))
            return fullName;
        else
            return type.AssemblyQualifiedName;
    }

    /// <summary>
    /// IsInheritBase
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <param name="includeInterfaces"></param>
    /// <returns></returns>
    public static bool IsInheritBase(Type type, Type baseType, bool includeInterfaces)
    {
        return GetInheritBaseType(type, baseType, includeInterfaces) != null;
    }

    /// <summary>
    /// GetInheritBase
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <param name="includeInterfaces"></param>
    /// <returns></returns>
    public static Type GetInheritBaseType(Type type, Type baseType, bool includeInterfaces)
    {
        if (baseType == null || type == null)
            return null;
        else if (type == baseType)
            return type;
        else if (type.IsGenericType && type.GetGenericTypeDefinition() == baseType)
            return type;

        Type tempBaseType = type.BaseType;
        while (tempBaseType != null)
        {
            if (tempBaseType == baseType)
                return tempBaseType;
            else
                tempBaseType = tempBaseType.BaseType;
        };

        if (includeInterfaces)
            return type.FindInterfaces((m, o) => m == baseType || m.IsGenericType && m.GetGenericTypeDefinition() == baseType, null).FirstOrDefault();

        return null;
    }

    /// <summary>
    /// IsIListT
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public static bool IsIListT(Type type)
    {
        if (type.IsArray || type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IList<>))
            return true;

        return type.FindInterfaces((m, o) => m.IsGenericType && m.GetGenericTypeDefinition() == typeof(IList<>), null).Count() > 0;
    }

    /// <summary>
    /// GetArrayItemType
    /// </summary>
    /// <param name="type">The type.</param>
    /// <returns></returns>
    public static Type GetArrayItemType(Type type)
    {
        if (!type.IsArray)
            return null;
        return type.GetElementType();
    }

    /// <summary>
    /// 枚举类型是否包含特定值
    /// </summary>
    /// <param name="enumType"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EnumContains(Type enumType, int value)
    {
        if (!enumType.IsEnum)
            throw new NotSupportedException("Can not support type: " + enumType.FullName);

        int[] values;
        if (!EnumTypeValues.TryGetValue(enumType, out values))
        {
            lock (EnumTypeValues)
            {
                if (!EnumTypeValues.TryGetValue(enumType, out values))
                {
                    values = Enum.GetValues(enumType).Cast<int>().ToArray();
                    EnumTypeValues[enumType] = values;
                }
            }
        }
        return values.Contains(value);
    }

    private static System.Collections.Generic.Dictionary<Type, int[]> EnumTypeValues = new System.Collections.Generic.Dictionary<Type, int[]>();

    /// <summary>
    /// Changes the type.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="conversionType">Type of the conversion.</param>
    /// <returns></returns>
    public static object ChangeType(object value, Type conversionType)
    {
        if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
                return null;

            System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);

            conversionType = nullableConverter.UnderlyingType;
        }

        return Convert.ChangeType(value, conversionType);
    }
}