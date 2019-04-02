using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebSite
{
    public class Util
    {
        /// <summary>
        /// HMAC-SHA1加密算法
        /// </summary>
        /// <param name="key">密钥</param>
        /// <param name="input">要加密的串</param>
        /// <returns></returns>
        public static string HmacSha1(string key, string input)
        {
            byte[] keyBytes = ASCIIEncoding.ASCII.GetBytes(key);
            byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(input);
            HMACSHA1 hmac = new HMACSHA1(keyBytes);
            byte[] hashBytes = hmac.ComputeHash(inputBytes);
            return Convert.ToBase64String(hashBytes);
        }
          
        public static Dictionary<string, int> GetCallRatioLevel()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            dictionary.Add("Level20", 20);
            dictionary.Add("Level30", 30);
            dictionary.Add("Level40", 40);
            dictionary.Add("Level50", 50);
            return dictionary;
        }
    }
}