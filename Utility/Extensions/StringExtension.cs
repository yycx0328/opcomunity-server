using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public static class StringExtension
{
    /// <summary>
    /// 基于Sha1的自定义加密字符串方法：输入一个字符串，
    /// 返回一个由40个字符组成的十六进制的哈希散列（字符串）。
    /// </summary>
    /// <param name="str">要加密的字符串</param>
    /// <returns>加密后的十六进制的哈希散列（字符串）</returns>
    public static string Sha1(this string str,bool tolower = true)
    {
        var buffer = Encoding.UTF8.GetBytes(str);
        var data = SHA1.Create().ComputeHash(buffer);

        var sb = new StringBuilder();
        foreach (var t in data)
        {
            sb.Append(t.ToString("X2"));
        }
        string result = sb.ToString();
        if (tolower)
            return result.ToLower();
        return result.ToUpper();
    }
}
