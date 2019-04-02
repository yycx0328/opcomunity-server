/*****************************************************
 * Author: 卢云海
 * Mail: lyh_oralce@hotmail.com
 * QQ: 1289927096
 * Date: 2017-04-20
 * **************************************************/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Common
{
    #region 加解密
    public class SecurityHelper
    { 
        #region 加密
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="userData">用户信息</param>
        /// <returns>加密字符串</returns>
        public static string Encrypt(string userData)
        {
            // 将用户数据存入票据对象
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, "vanluu", DateTime.Now, DateTime.Now.AddMinutes(10), true, userData);
            // 将票据对象加密成字符串
            string security = FormsAuthentication.Encrypt(ticket);
            return security;
        } 
        #endregion

        #region 解密
        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedTicket">加密字符串</param>
        /// <returns>用户数据</returns>
        public static string Decrypt(string encryptedTicket)
        {
            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(encryptedTicket);
            return ticket.UserData;
        }
        #endregion

        #region 3DES加解密
        #region 3DES加密
        /// <summary>
        ///3DES加密
        /// </summary>
        /// <param name="originalValue">加密数据</param>
        /// <param name="key">24位字符的密钥字符串</param>
        /// <param name="IV">8位字符的初始化向量字符串</param>
        /// <returns></returns>
        public static string TripleDESEncrypt(string originalValue, string key, string IV)
        {
            try
            {
                SymmetricAlgorithm sa;
                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;
                sa = new TripleDESCryptoServiceProvider();
                sa.Key = Encoding.UTF8.GetBytes(key);
                sa.IV = Encoding.UTF8.GetBytes(IV);
                ct = sa.CreateEncryptor();
                byt = Encoding.UTF8.GetBytes(originalValue);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        #endregion

        #region 3DES解密
        /// <summary>
        /// 3DES解密
        /// </summary>
        /// <param name="data">解密数据</param>
        /// <param name="key">24位字符的密钥字符串(需要和加密时相同)</param>
        /// <param name="iv">8位字符的初始化向量字符串(需要和加密时相同)</param>
        /// <returns></returns>
        public static string TripleDESDecrypst(string data, string key, string IV)
        {
            try
            {
                SymmetricAlgorithm mCSP = new TripleDESCryptoServiceProvider();
                mCSP.Key = Encoding.UTF8.GetBytes(key);
                mCSP.IV = Encoding.UTF8.GetBytes(IV);
                ICryptoTransform ct;
                MemoryStream ms;
                CryptoStream cs;
                byte[] byt;
                ct = mCSP.CreateDecryptor(mCSP.Key, mCSP.IV);
                byt = Convert.FromBase64String(data);
                ms = new MemoryStream();
                cs = new CryptoStream(ms, ct, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();
                cs.Close();
                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        #endregion 
        #endregion
    } 
    #endregion
}
