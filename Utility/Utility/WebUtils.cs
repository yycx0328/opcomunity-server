using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;

/// <summary>
/// WebUtils
/// </summary>
public static class WebUtils
{
    private const string sContentType = "application/x-www-form-urlencoded";

    /// <summary>
    /// Initializes the <see cref="WebUtils"/> class.
    /// </summary>
    static WebUtils()
    {
        System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(
            delegate (Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            });
    }

    #region 获取客户端IP地址
    /// <summary>
    /// 获取客户端IP地址
    /// </summary>
    /// <returns></returns>
    public static string GetClientIP()
    {
        if (HttpContext.Current == null)
        {
            //WCF
            if (OperationContext.Current == null)
                return string.Empty;

            var request = OperationContext.Current.RequestContext;
            if (request == null || request.RequestMessage == null)
                return string.Empty;

            RemoteEndpointMessageProperty remote = request.RequestMessage.Properties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
            if (remote == null)
                return string.Empty;
            else
                return remote.Address;
        }
        else
        {
            var request = HttpContext.Current.Request;
            if (request == null)
                return string.Empty;

            string ipList = TypeHelper.TryParse(request.ServerVariables["HTTP_X_FORWARDED_FOR"], request.ServerVariables["REMOTE_ADDR"]);
            string ipAddress = ipList.Split(',', ';')[0];

            return TypeHelper.TryParse(ipAddress, request.UserHostAddress);
        }
    } 
    #endregion
    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientIP"></param>
    /// <returns></returns>
    public static string GetClientComputerName(string clientIP)
    {
        try
        {
            var hostEntry = Dns.GetHostEntry(clientIP);
            return hostEntry.HostName;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            return string.Empty;
        }
    }

    /// <summary>
    /// 获取User的浏览器名称
    /// </summary>
    /// <returns></returns>
    public static string GetClientBrowser()
    {
        if (HttpContext.Current == null)
            return string.Empty;
        var request = HttpContext.Current.Request;
        if (request == null)
            return string.Empty;

        if (request.Browser == null)
            return string.Empty;
        else
            return request.Browser.Browser + " " + request.Browser.Version;
    }

    public static string GetPathUrl(System.Uri uri)
    {
        if (uri.Port == 80)
            return string.Format("{0}://{1}{2}", uri.Scheme, uri.Host, uri.AbsolutePath);
        else
            return string.Format("{0}://{1}:{2}{3}", uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath);
    }

    /// <summary>
    /// 获取用户操作系统
    /// </summary>
    /// <returns></returns>
    public static string GetClientOS()
    {
        string osVersion = "未知";
        if (HttpContext.Current == null)
            return osVersion;
        var request = HttpContext.Current.Request;
        if (request == null)
            return osVersion;

        if (request.UserAgent == null)
            return osVersion;
        var userAgent = request.UserAgent;
        if (userAgent.Contains("NT 6.1"))
        {
            osVersion = "Windows 7";
        }
        else if (userAgent.Contains("NT 6.0"))
        {
            osVersion = "Windows Vista/Server 2008";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osVersion = "Windows Server 2003";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osVersion = "Windows XP";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osVersion = "Windows 2000";
        }
        else if (userAgent.Contains("NT 4"))
        {
            osVersion = "Windows NT4";
        }
        else if (userAgent.Contains("Me"))
        {
            osVersion = "Windows Me";
        }
        else if (userAgent.Contains("98"))
        {
            osVersion = "Windows 98";
        }
        else if (userAgent.Contains("95"))
        {
            osVersion = "Windows 95";
        }
        else if (userAgent.Contains("Mac"))
        {
            osVersion = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osVersion = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osVersion = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osVersion = "SunOS";
        }
        return osVersion;
    }

    /// <summary>
    /// 获取用户系统内核
    /// </summary>
    /// <returns></returns>
    public static string GetClientOSCore()
    {
        string osCore = "未知";
        if (HttpContext.Current == null)
            return osCore;
        var request = HttpContext.Current.Request;
        if (request == null)
            return osCore;

        if (request.UserAgent == null)
            return osCore;
        var userAgent = request.UserAgent;
        if (userAgent.Contains("NT 6.1"))
        {
            osCore = "NT 6.1";
        }
        else if (userAgent.Contains("NT 6.0"))
        {
            osCore = "NT 6.0";
        }
        else if (userAgent.Contains("NT 5.2"))
        {
            osCore = "NT 5.2";
        }
        else if (userAgent.Contains("NT 5.1"))
        {
            osCore = "NT 5.1";
        }
        else if (userAgent.Contains("NT 5"))
        {
            osCore = "NT 5";
        }
        else if (userAgent.Contains("NT 4"))
        {
            osCore = "NT 4";
        }
        else if (userAgent.Contains("Me"))
        {
            osCore = "Me";
        }
        else if (userAgent.Contains("98"))
        {
            osCore = "98";
        }
        else if (userAgent.Contains("95"))
        {
            osCore = "95";
        }
        else if (userAgent.Contains("Mac"))
        {
            osCore = "Mac";
        }
        else if (userAgent.Contains("Unix"))
        {
            osCore = "UNIX";
        }
        else if (userAgent.Contains("Linux"))
        {
            osCore = "Linux";
        }
        else if (userAgent.Contains("SunOS"))
        {
            osCore = "SunOS";
        }
        return osCore;
    }

    /// <summary>
    /// 判断是否为有效的url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static bool IsURL(string url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }
        String myregex = @"(([\w]+:)?//)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?";
        return System.Text.RegularExpressions.Regex.IsMatch(url, myregex);
    }

    /// <summary>
    /// 获取url的host+port
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetUrlHostPort(string url)
    {
        if (!IsURL(url))
            return url;
        try
        {
            Uri uri = new Uri(url);
            int port = 0;
            if (!uri.IsDefaultPort)
            {
                port = uri.Port;
            }
            if (port == 0)
            {
                url = uri.Host;
            }
            else
            {
                url = uri.Host + ":" + port;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error:" + e.ToString());
        }
        return url;
    }

    /// <summary>
    /// 获取访问地址
    /// </summary>
    /// <param name="maxLength">Length of the max.</param>
    /// <returns></returns>
    public static string GetClientUrl(int maxLength)
    {
        var ctx = HttpContext.Current;
        if (ctx == null)
            return string.Empty;

        if (ctx.Request != null && ctx.Request.Url != null)
        {
            return TypeHelper.GetSubString(ctx.Request.Url.ToString(), maxLength);
        }
        else
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// 是否允许用户的IP
    /// </summary>
    /// <param name="filter">The filter.</param>
    /// <param name="userIP">The user IP.</param>
    /// <returns>
    /// 	<c>true</c> if [is allowed IP] [the specified filter]; otherwise, <c>false</c>.
    /// </returns>
    public static bool IsAllowedIP(string filter, string userIP)
    {
        string[] ipList = WebCache.GetCachedObject<string[]>(filter,
            delegate ()
            {
                XElement node = XElement.Load(filter);
                var list = node.Elements("allow").Elements("ip").Select(o => o.Value).Select(p => p.Replace(" ", "")).ToList();
                list.Sort();
                return list.ToArray();
            });

        return Arithmetic.IsHazyMatched(ipList, userIP, '.');
    }

    #region GetPostValue
    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static string GetPostValue(System.Web.HttpRequest request, string name, string defaultValue)
    {
        string result = request.Form[name];
        if (string.IsNullOrEmpty(result))
        {
            result = request.QueryString[name];
            if (string.IsNullOrEmpty(result))
                return defaultValue;
        }
        return result;
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <returns></returns>
    public static string GetPostValue(System.Web.HttpRequest request, string name)
    {
        string result = request.Form[name];
        if (string.IsNullOrEmpty(result))
        {
            result = request.QueryString[name];
            if (string.IsNullOrEmpty(result))
                return string.Empty;
        }
        return result;
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static int GetPostValue(System.Web.HttpRequest request, string name, int defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static long GetPostValue(System.Web.HttpRequest request, string name, long defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static DateTime GetPostValue(System.Web.HttpRequest request, string name, DateTime defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static int[] GetPostValue(System.Web.HttpRequest request, string name, int[] defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static Guid GetPostValue(System.Web.HttpRequest request, string name, Guid defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static Enum GetPostValue(System.Web.HttpRequest request, string name, Enum defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }

    /// <summary>
    /// GetPostValue
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="name">The name.</param>
    /// <param name="defaultValue">The default value.</param>
    /// <returns></returns>
    public static bool GetPostValue(System.Web.HttpRequest request, string name, bool defaultValue)
    {
        return TypeHelper.TryParse(GetPostValue(request, name), defaultValue);
    }
    #endregion

    #region get the text of url page


    /// <summary>
    /// 返回页面内容
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="encoding">The encoding.</param>
    /// <returns></returns>
    public static string GetUrlOutput(string url, System.Text.Encoding encoding)
    {
        return GetUrlOutput(url, encoding, 90000);
    }

    /// <summary>
    /// 返回页面内容
    /// </summary>
    /// <param name="url"></param>
    /// <param name="timeOut"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetUrlOutput(string url, System.Text.Encoding encoding, int timeOut)
    {
        //设置HttpWebRequest
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "GET";
        request.KeepAlive = false;
        request.Timeout = timeOut;

        try
        {
            //获取数据
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse()))
            using (System.IO.Stream stream = response.GetResponseStream())
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, encoding))
            {
                string result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                response.Close();
                return result;
            }
        }
        finally
        {
            request.Abort();
        }
    }

    /// <summary>
    /// 获取url 跳转的地址参数
    /// </summary>
    /// <param name="url"></param>
    /// <param name="encoding"></param>
    /// <param name="timeOut"></param>
    /// <returns></returns>
    public static string GetUrlResponseQuery(string url, System.Text.Encoding encoding)
    {
        //设置HttpWebRequest
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "GET";
        request.KeepAlive = false;
        request.Timeout = 90000;

        try
        {
            //获取数据
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse()))
            {
                string result = response.ResponseUri.Query;
                response.Close();
                return result;
            }
        }
        finally
        {
            request.Abort();
        }
    }

    /// <summary>
    /// 返回页面内容
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <returns></returns>
    public static string GetUrlOutput(string url)
    {
        return GetUrlOutput(url, System.Text.Encoding.UTF8);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool TryParseWebContent(string requestUrl, out string content)
    {
        return TryParseWebContent(requestUrl, Encoding.Default, false, 5000, out content);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="isSSL"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool TryParseWebContent(string requestUrl, bool isSSL, out string content)
    {
        return TryParseWebContent(requestUrl, Encoding.Default, isSSL, 5000, out content);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="encoding"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool TryParseWebContent(string requestUrl, Encoding encoding, out string content)
    {
        return TryParseWebContent(requestUrl, encoding, false, 5000, out content);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="encoding"></param>
    /// <param name="isSSL"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool TryParseWebContent(string requestUrl, Encoding encoding, bool isSSL, out string content)
    {
        return TryParseWebContent(requestUrl, encoding, isSSL, 5000, out content);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="requestUrl"></param>
    /// <param name="encoding"></param>
    /// <param name="isSSL"></param>
    /// <param name="timeOut"></param>
    /// <param name="content"></param>
    /// <returns></returns>
    public static bool TryParseWebContent(string requestUrl, Encoding encoding, bool isSSL, int timeOut, out string content)
    {
        content = string.Empty;
        try
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(requestUrl);
            request.Headers.Add("Accept-Language", "cs,en-us;q=0.7,en;q=0.3");
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0;Windows NT 5.0; .NET CLR 1.0.3705; .NET CLR 1.1.4322)";

            request.Timeout = timeOut;

            if (isSSL)
            {
                NetworkCredential credentials = new NetworkCredential(string.Empty, string.Empty, string.Empty);
                request.Credentials = credentials;
            }

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream stream;
            if (response.ContentEncoding == "gzip")
            {
                stream = new GZipStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else if (response.ContentEncoding == "deflate")
            {
                stream = new DeflateStream(response.GetResponseStream(), CompressionMode.Decompress);
            }
            else
            {
                stream = response.GetResponseStream();
            }
            try
            {
                using (StreamReader reader = new StreamReader(stream, encoding))
                {
                    content = reader.ReadToEnd();
                }
                stream.Close();
                stream.Dispose();
            }
            catch
            {
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
        }
        catch (Exception ex)
        {

            content = ex.Message;
            return false;
        }

        return true;
    }

    #endregion

    /// <summary>
    /// POST数据到指定地址
    /// </summary>
    /// <param name="url"></param>
    /// <param name="encoding"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static string PostDataToUrl(string url, System.Text.Encoding encoding, System.Collections.Specialized.NameValueCollection data)
    {
        //设置HttpWebRequest
        System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
        request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.2; .NET CLR 1.1.4322; .NET CLR 2.0.50727)";
        request.ContentType = "application/x-www-form-urlencoded";
        request.Method = "POST";
        request.KeepAlive = false;

        byte[] buffer;
        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
        {
            for (int i = 0; i < data.Count; i++)
            {
                buffer = encoding.GetBytes(data.Keys[i]);
                if (i > 0)
                    ms.WriteByte(Convert.ToByte('&'));
                ms.Write(buffer, 0, buffer.Length);
                ms.WriteByte(Convert.ToByte('='));
                buffer = encoding.GetBytes(HttpUtility.UrlEncode(data[i]));
                ms.Write(buffer, 0, buffer.Length);
            }
            buffer = ms.ToArray();
        }

        request.ContentLength = buffer.Length;
        using (System.IO.Stream requestStream = request.GetRequestStream())
        {
            requestStream.Write(buffer, 0, buffer.Length);
            requestStream.Close();
        }

        try
        {
            //获取数据
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse()))
            using (System.IO.Stream stream = response.GetResponseStream())
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream, encoding))
            {
                string result = reader.ReadToEnd();
                reader.Close();
                stream.Close();
                response.Close();
                return result;
            }
        }
        finally
        {
            request.Abort();
        }
    }

    public static string Send(string data, string url)
    {
        return Send(Encoding.GetEncoding("UTF-8").GetBytes(data), url);
    }

    public static string Send(byte[] data, string url)
    {
        Stream responseStream;
        HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
        if (request == null)
        {
            throw new ApplicationException(string.Format("Invalid url string: {0}", url));
        }
        request.ContentType = sContentType;
        request.Method = "POST";
        request.ContentLength = data.Length;
        Stream requestStream = request.GetRequestStream();
        requestStream.Write(data, 0, data.Length);
        requestStream.Close();
        try
        {
            responseStream = request.GetResponse().GetResponseStream();
        }
        catch (Exception exception)
        {
            throw exception;
        }
        string str = string.Empty;
        using (StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
        {
            str = reader.ReadToEnd();
        }
        responseStream.Close();
        return str;
    }

    /// <summary>
    /// 获取发出请求，并获取状态码
    /// </summary>
    /// <param name="url"></param>
    /// <param name="encoding">System.Text.Encoding</param>
    /// <returns></returns> 
    public static int GetHttpStatusCode(string url, Encoding encoding)
    {
        HttpWebRequest request = null;
        HttpWebResponse response = null;
        try
        {
            request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = HttpContext.Current.Request.Url.Host.ToString();
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            response = (HttpWebResponse)request.GetResponse();
            return (int)response.StatusCode;
        }
        catch { return 0; }
        finally
        {
            if (response != null) { response.Close(); response = null; }
            if (request != null) { request = null; }
        }
    }

    /// <summary>
    /// Accepts all certificate policy.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="certificate">The certificate.</param>
    /// <param name="chain">The chain.</param>
    /// <param name="sslPolicyErrors">The SSL policy errors.</param>
    /// <returns></returns>
    public static bool AcceptAllCertificatePolicy(Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
    {
        //Return True to force the certificate to be accepted. 
        return true;
    }



    /// <summary>
    /// BuildUrl
    /// </summary>
    /// <param name="url">The URL.</param>
    /// <param name="appendParameters">The append parameters.</param>
    /// <returns></returns>
    public static string BuildUrl(string url, NameValueCollection appendParameters)
    {
        if (string.IsNullOrEmpty(url))
            return string.Empty;

        string[] parts = url.Split(new char[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries);
        NameValueCollection queryString = new NameValueCollection();
        for (int i = 1; i < parts.Length; i++)
        {
            string[] temp = parts[i].Split('=');
            if (temp.Length == 2)
            {
                queryString[temp[0]] = temp[1];
            }
        }

        for (int i = 0; i < appendParameters.Count; i++)
        {
            queryString[appendParameters.AllKeys[i]] = appendParameters[i];
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder(parts[0]);
        for (int i = 0; i < queryString.Count; i++)
        {
            if (i == 0)
                sb.AppendFormat("?{0}={1}", queryString.AllKeys[i], System.Web.HttpUtility.UrlEncode(queryString[i]));
            else
                sb.AppendFormat("&{0}={1}", queryString.AllKeys[i], System.Web.HttpUtility.UrlEncode(queryString[i]));
        }

        return sb.ToString();
    }

    /// <summary>
    /// MD5
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string MD5(string input)
    {
        return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(input, "MD5");
    }

    public static string MD5(string input, string input_charset)
    {
        StringBuilder sb = new StringBuilder(32);
        MD5 md5 = new MD5CryptoServiceProvider();
        byte[] t = md5.ComputeHash(Encoding.GetEncoding(input_charset).GetBytes(input));
        for (int i = 0; i < t.Length; i++)
        {
            sb.Append(t[i].ToString("x").PadLeft(2, '0'));
        }
        return sb.ToString();
    }

    public static string GetMD5(string encypStr, string charset)
    {
        string retStr;
        MD5CryptoServiceProvider m5 = new MD5CryptoServiceProvider();

        //创建md5对象
        byte[] inputBye;
        byte[] outputBye;

        //使用GB2312编码方式把字符串转化为字节数组．
        try
        {
            inputBye = Encoding.GetEncoding(charset).GetBytes(encypStr);
        }
        catch (Exception ex)
        {
            inputBye = Encoding.GetEncoding("GB2312").GetBytes(encypStr);
        }
        outputBye = m5.ComputeHash(inputBye);

        retStr = System.BitConverter.ToString(outputBye);
        retStr = retStr.Replace("-", "").ToUpper();
        return retStr;
    }

    /** 对字符串进行URL编码 */
    public static string UrlEncode(string instr, string charset)
    {
        //return instr;
        if (instr == null || instr.Trim() == "")
            return "";
        else
        {
            string res;
            try
            {
                res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding(charset));
            }
            catch (Exception ex)
            {
                res = HttpUtility.UrlEncode(instr, Encoding.GetEncoding("GB2312"));
            }

            return res;
        }
    }

    public static String MD5EncryptToUpperUTF8(String originalText)
    {
        byte[] originalByte = Encoding.UTF8.GetBytes(originalText);
        System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] outputByte = md5.ComputeHash(originalByte);
        return BitConverter.ToString(outputByte).Replace("-", "").ToUpper();
    }

    public static string GetMD5Hash(Stream stream)
    {
        string strResult = "";
        string strHashData = "";
        byte[] arrbytHashValue;
        MD5CryptoServiceProvider oMD5Hasher = new MD5CryptoServiceProvider();
        try
        {
            arrbytHashValue = oMD5Hasher.ComputeHash(stream);//计算指定Stream 对象的哈希值
                                                             //由以连字符分隔的十六进制对构成的String，其中每一对表示value 中对应的元素；例如“F-2C-4A”
            strHashData = System.BitConverter.ToString(arrbytHashValue);
            //替换-
            strHashData = strHashData.Replace("-", "");
            strResult = strHashData.ToLower();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
        }
        return strResult;
    }

    public static string HMAC_MD5(string key, string input)
    {
        byte[] keys = Encoding.Default.GetBytes(key);
        using (System.Security.Cryptography.HMACMD5 hmac = new HMACMD5(keys))
        {
            byte[] computedHash = hmac.ComputeHash(Encoding.Default.GetBytes(input));
            StringBuilder result = new StringBuilder();
            for (int counter = 0; counter < computedHash.Length; counter++)
            {
                result.Append(computedHash[counter].ToString("x2"));
            }
            return result.ToString();
        }
    }

    public static string byteToHexStr(byte[] bytes)
    {
        string returnStr = "";
        if (bytes != null)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                returnStr += bytes[i].ToString("X2");
            }
        }
        return returnStr;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="certificateFileName">一个证书文件的名称</param>
    /// <param name="password">访问 X.509 证书数据所需的密码</param>
    /// <returns></returns>
    public static string RSA(string input, string certificateFileName, string password)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        X509Certificate2 cert = new X509Certificate2(certificateFileName, password, X509KeyStorageFlags.MachineKeySet);
        RSACryptoServiceProvider rsapri = (RSACryptoServiceProvider)cert.PrivateKey;
        RSAPKCS1SignatureFormatter f = new RSAPKCS1SignatureFormatter(rsapri);
        byte[] result;
        f.SetHashAlgorithm("SHA1");
        SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
        result = sha.ComputeHash(bytes);
        return System.Convert.ToBase64String(f.CreateSignature(result)).ToString();
    }

    public static bool RSAVerifySignature(string input, string compareSign, string certificateFileName)
    {
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        byte[] SignatureByte = Convert.FromBase64String(compareSign);
        X509Certificate2 cert = new X509Certificate2(certificateFileName, "");
        RSACryptoServiceProvider rsapri = (RSACryptoServiceProvider)cert.PublicKey.Key;
        rsapri.ImportCspBlob(rsapri.ExportCspBlob(false));
        RSAPKCS1SignatureDeformatter f = new RSAPKCS1SignatureDeformatter(rsapri);
        byte[] result;
        f.SetHashAlgorithm("SHA1");
        SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
        result = sha.ComputeHash(bytes);
        if (f.VerifySignature(result, SignatureByte))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// UrlDecode
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string UrlDecode(string input)
    {
        return System.Web.HttpUtility.UrlDecode(input);
    }
    /// <summary>
    /// 判断页面返回类型
    /// </summary>
    /// <param name="url"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string GetContentType(string url, System.Text.Encoding encoding)
    {
        string strResult = "";
        //设置HttpWebRequest
        try
        {
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.KeepAlive = false;

            //获取验证数据
            using (System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)(request.GetResponse()))
            {
                if (response.ContentType.IndexOf("image") > -1)
                {
                    strResult = "image";
                }
                else
                {
                    strResult = string.Empty;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            strResult = string.Empty;
        }
        return strResult;
    }

    /// <summary>
    /// 将domain-userid替换成domain\userid
    /// </summary>
    /// <param name="Domain"></param>
    /// <returns></returns>
    public static string ToDomain(string Domain)
    {
        if (Domain != null)
        {
            Domain = Domain.Replace("-", "\\");
        }
        return Domain;
    }

    #region 获取所需要的页面内容

    /// <summary>
    /// 获取页面编码类型
    /// </summary>
    /// <param name="strUrl"></param>
    /// <param name="timeout"></param>
    /// <returns></returns>
    public static Encoding GetHtmlEncodeType(string strUrl, int timeout)
    {
        if (strUrl.Equals("about:blank")) return null; ;
        if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("https://")) { strUrl = "http://" + strUrl; }
        string strResult = string.Empty;

        Encoding encodeType = Encoding.UTF8;

        System.IO.StreamReader sr = null;
        string temp = string.Empty;
        try
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);//strUrl
            myReq.Timeout = timeout;
            myReq.UserAgent = "User-Agent:Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 2.0.40607; .NET CLR 1.1.4322; .NET CLR 3.5.30729)";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            if (HttpWResp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StringBuilder strBuilder = new StringBuilder();
                Stream myStream = HttpWResp.GetResponseStream();
                sr = new StreamReader(myStream, encodeType);
                string tmp = string.Empty;
                while ((temp = sr.ReadLine()) != null)
                {
                    strBuilder.Append(temp);
                    tmp = strBuilder.ToString();
                    //if (tmp.IndexOf("charset") > 0)
                    //{
                    //    //  判断编码, 如果不是utf-8则进行转码
                    //    encodeType = GetEncoding(strBuilder.ToString());
                    //    break;
                    //}
                    if (tmp.IndexOf("charset") > 0)
                    {
                        System.Text.RegularExpressions.Regex reg_charset = new System.Text.RegularExpressions.Regex(@"charset\b\s*=\s*(?<charset>[^""]*)");
                        if (reg_charset.IsMatch(tmp))
                        {
                            var metaCharset = reg_charset.Match(tmp).Groups["charset"].Value;
                            if (metaCharset != "")
                            {
                                encodeType = Encoding.GetEncoding(metaCharset);
                            }
                            else
                            {
                                encodeType = Encoding.GetEncoding(HttpWResp.CharacterSet);
                            }
                        }
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            return encodeType;
        }
        finally
        {
            if (sr != null) { sr.Close(); }
        }
        return encodeType;
    }

    /// <summary>
    /// 获取所需要的页面内容
    /// </summary>
    /// <param name="strUrl">所要查找的远程网页地址</param>
    /// <param name="timeout">超时时长设置，一般设置为8000</param>
    /// <param name="enterType">是否输出换行符，0不输出，1输出文本框换行</param>
    /// <param name="endTag"></param>
    public static string GetHttpRequestString(string strUrl, int timeout, int enterType, string endTag)
    {

        if (strUrl.Equals("about:blank")) return null; ;
        if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("https://")) { strUrl = "http://" + strUrl; }
        string strResult = string.Empty;

        //读取编码
        //Encoding encodeType = GetHtmlEncodeType(strUrl, timeout);

        System.IO.StreamReader sr = null;
        string temp = string.Empty;
        try
        {
            HttpWebRequest myReq = (HttpWebRequest)HttpWebRequest.Create(strUrl);//strUrl
            myReq.Timeout = timeout;
            myReq.UserAgent = "User-Agent:Mozilla/5.0 (compatible; MSIE 6.0; Windows NT 5.2; SV1; .NET CLR 2.0.40607; .NET CLR 1.1.4322; .NET CLR 3.5.30729)";
            myReq.Accept = "*/*";
            myReq.KeepAlive = true;
            myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
            myReq.Method = "GET";
            HttpWebResponse HttpWResp = (HttpWebResponse)myReq.GetResponse();
            if (HttpWResp.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StringBuilder strBuilder = new StringBuilder();
                Stream myStream = HttpWResp.GetResponseStream();

                var charset = Encoding.UTF8.ToString();
                //if (HttpWResp.CharacterSet == "ISO-8859-1")
                //{
                //    charset = "gb2312";
                //}
                //else
                //{
                //    charset = HttpWResp.CharacterSet;
                //}
                Encoding encodeType = GetHtmlEncodeType(strUrl, timeout);

                sr = new StreamReader(myStream, encodeType);

                string tmp = string.Empty;
                while ((temp = sr.ReadLine()) != null)
                {
                    strBuilder.Append(temp);
                    tmp = strBuilder.ToString();
                    if (tmp.IndexOf(endTag) > 0) { break; }
                    if (enterType == 1) { strBuilder.Append("\r\n"); }
                }
                strResult = strBuilder.ToString();

                return strResult;
            }
            return string.Empty;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            return strResult;
        }
        finally
        {
            if (sr != null) { sr.Close(); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strUrl"></param>
    /// <param name="timeout"></param>
    /// <param name="enterType"></param>
    /// <param name="endTag"></param>
    /// <returns></returns>
    public static string GetWebRequestString(string strUrl, int timeout, int enterType, string endTag)
    {

        if (strUrl.Equals("about:blank")) return null; ;
        if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("https://")) { strUrl = "http://" + strUrl; }
        string strResult = string.Empty;

        //读取编码
        //Encoding encodeType = GetHtmlEncodeType(strUrl, timeout);
        strUrl = HttpUtility.UrlEncode(strUrl);
        System.IO.StreamReader sr = null;
        string temp = string.Empty;
        WebRequest request = WebRequest.Create(strUrl);
        try
        {
            using (WebResponse response = request.GetResponse())
            {
                StringBuilder strBuilder = new StringBuilder();
                Stream myStream = response.GetResponseStream();

                var charset = "utf-8";
                //if (response. == "ISO-8859-1")
                //{
                //    charset = "gb2312";
                //}
                //else
                //{
                //    charset = response.CharacterSet;
                //}
                Encoding encodeType = Encoding.GetEncoding(charset);

                sr = new StreamReader(myStream, encodeType);

                string tmp = string.Empty;
                while ((temp = sr.ReadLine()) != null)
                {
                    strBuilder.Append(temp);
                    tmp = strBuilder.ToString();
                    if (tmp.IndexOf(endTag) > 0) { break; }
                    if (enterType == 1) { strBuilder.Append("\r\n"); }
                }
                strResult = strBuilder.ToString();
            }
        }
        catch (WebException e)
        {
            using (WebResponse response = e.Response)
            {
                HttpWebResponse httpResponse = (HttpWebResponse)response;
                Console.WriteLine("Error code: {0}", httpResponse.StatusCode);
                using (Stream data = response.GetResponseStream())
                {
                    strResult = new StreamReader(data).ReadToEnd();
                }
            }
        }
        finally
        {
            if (sr != null) { sr.Close(); }
        }
        return strResult;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="strUrl"></param>
    /// <returns></returns>
    public static string GetWebRequestHtml(string strUrl)
    {
        if (strUrl.Equals("about:blank")) return null; ;
        if (!strUrl.StartsWith("http://") && !strUrl.StartsWith("https://")) { strUrl = "http://" + strUrl; }
        string strResult = string.Empty;
        //strUrl = HttpUtility.UrlEncode(strUrl);
        System.IO.StreamReader sr = null;
        string temp = string.Empty;
        WebRequest request = WebRequest.Create(strUrl);
        request.Method = "Post";
        try
        {
            using (WebResponse response = request.GetResponse())
            {
                StringBuilder strBuilder = new StringBuilder();
                Stream myStream = response.GetResponseStream();
                var charset = "utf-8";
                Encoding encodeType = Encoding.GetEncoding(charset);

                sr = new StreamReader(myStream, encodeType);

                strResult = sr.ReadToEnd();
            }
        }
        catch (WebException e)
        {
            Console.WriteLine("Error:" + e.ToString());
        }
        finally
        {
            if (sr != null) { sr.Close(); }
        }
        return strResult;
    }

    /// <summary>
    /// 获取html所有img src
    /// </summary>
    /// <param name="sHtmlText"></param>
    /// <returns></returns>
    public static string[] GetHtmlImageUrlList(string sHtmlText)
    {
        // 定义正则表达式用来匹配 img 标签 
        Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

        // 搜索匹配的字符串 
        MatchCollection matches = regImg.Matches(sHtmlText);
        int i = 0;
        string[] sUrlList = new string[matches.Count];

        // 取得匹配项列表 
        foreach (Match match in matches)
            sUrlList[i++] = match.Groups["imgUrl"].Value;
        return sUrlList;
    }

    /// <summary>
    /// 获取网页title
    /// </summary>
    /// <param name="sHtml"></param>
    /// <returns></returns>
    public static string GetTitleContextByHtml(string sHtml)
    {
        try
        {
            //        //获取标题
            //Match TitleMatch = Regex.Match(strResponse, "<title>([^<]*)</title>", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            //title = TitleMatch.Groups[1].Value;

            string regex = @"(?<=<title.*>)([\s\S]*)(?=</title>)";
            System.Text.RegularExpressions.Regex ex = new System.Text.RegularExpressions.Regex(regex, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return ex.Match(sHtml).Value.Trim();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            return "unknow";
        }
    }

    /// <summary>
    /// 获取描述信息
    /// </summary>
    /// <param name="sHtml"></param>
    /// <returns></returns>
    public static string GetMetaDescriptionByHtml(string sHtml)
    {
        string strDescription = "";
        try
        {
            //获取描述信息
            System.Text.RegularExpressions.Match Desc;
            Desc = System.Text.RegularExpressions.Regex.Match(sHtml, "<meta content=\"([^<]*)\" name=\"description\"+/?>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
            strDescription = Desc.Groups[1].Value;
            if (string.IsNullOrEmpty(strDescription))
            {
                Desc = System.Text.RegularExpressions.Regex.Match(sHtml, "<meta name=\"description\" content=\"([^<]*)\"\\s?/?>",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Multiline);
                strDescription = Desc.Groups[1].Value;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            return "";
        }
        return strDescription;
    }

    /// <summary>
    /// 根据网页的HTML内容提取网页的Encoding
    /// </summary>
    /// <param name="html"></param>
    /// <returns></returns>
    public static Encoding GetEncoding(string html)
    {
        string pattern = @"(?i)\bcharset=(?<charset>[-a-zA-Z_0-9]+)";
        string charset = System.Text.RegularExpressions.Regex.Match(html, pattern).Groups["charset"].Value;
        try { return Encoding.GetEncoding(charset); }
        catch (ArgumentException) { return null; }
    }

    /// <summary>
    /// 根据Url获取title
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string GetUrlTitle(string url)
    {
        string html = GetHttpRequestString(url, 8000, 0, "</title>");
        string title = GetTitleContextByHtml(html);
        if (string.IsNullOrEmpty(title))
        {
            html = GetHttpRequestString(url, 8000, 0, "</title>");
            title = GetTitleContextByHtml(html);
        }
        return title;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="url"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    public static void GetTitleAndDesdByUrl(string url, out string title, out string description)
    {
        string html = GetHttpRequestString(url, 8000, 0, "</head>");
        //string html = GetWebRequestString(url, 8000, 0, "</head>");
        title = GetTitleContextByHtml(html);
        description = GetMetaDescriptionByHtml(html);
    }

    /// <summary>
    /// 根据Url获取错误信息
    /// </summary>
    /// <param name="curl"></param>
    /// <returns></returns>
    public static int GetUrlError(string curl)
    {
        int num = 200;
        try
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(curl));
            request.Method = "head";
            ServicePointManager.Expect100Continue = false;

            ((HttpWebResponse)request.GetResponse()).Close();
        }
        catch (WebException exception)
        {
            if (exception.Status != WebExceptionStatus.ProtocolError)
            {
                return num;
            }
            if (exception.Message.IndexOf("500") > 0)
            {
                return 500;
            }
            if (exception.Message.IndexOf("401") > 0)
            {
                return 401;
            }
            if (exception.Message.IndexOf("404") > 0)
            {
                num = 404;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error:" + ex.ToString());
            num = 407;
        }
        return num;
    }

    public static string GetAbsoluteUrl(HttpRequest request, string pagePath)
    {
        return string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, pagePath);
    }
    #endregion

    #region calendarnotepad html转义
    /// <summary>
    /// 转换HTML字符
    /// </summary>
    /// <param name="source">目标字符串</param>
    /// <returns>返回结果</returns>
    public static string ToHTML(string source)
    {
        if (source != null)
        {
            source = source.Replace("&", "&amp;");
            source = source.Replace("<", "&lt;");
            source = source.Replace(">", "&gt;");
            source = source.Replace(" ", "&nbsp;");
            source = source.Replace("\n", "<br>");
            source = source.Replace("\"", "&quot;");
        }
        return source;
    }

    /// <summary>
    /// JavaScript语句过滤
    /// </summary>
    /// <param name="source">目标字符串</param>
    /// <returns>返回结果</returns>
    public static string ToJS(string source)
    {
        if (source != null)
        {
            source = source.Replace(@"\", @"\\");
            source = source.Replace(@"'", @"\'");
            source = source.Replace("\"", "\\\"");
            source = source.Replace("\r\n", @"\n");
            source = source.Replace("\n", @"\n");
            source = source.Replace("\r", @"\n");
        }
        return source;
    }

    /// <summary>
    /// 去除字符串末尾字符
    /// </summary>
    /// <param name="source">目标字符串</param>
    /// <param name="trimStr">要去除的字符串</param>
    public static bool TrimEnd(StringBuilder source, string trimStr)
    {
        bool bolResult = true;
        if (source.Length < trimStr.Length)
        {
            return false;
        }
        for (int i = 0; i < trimStr.Length; i++)
        {
            if (source[source.Length - trimStr.Length + i] != trimStr[i])
            {
                bolResult = false;
                break;
            }
        }
        if (bolResult)
        {
            source.Remove(source.Length - trimStr.Length, trimStr.Length);
            TrimEnd(source, trimStr);
        }
        return bolResult;
    }
    #endregion

    #region =====DES加密/解密类=======
    #region ========加密========
    /// <summary>
    /// 加密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string EncryptDES(string Text)
    {
        return Encrypt(Text, "luyunhai");
    }

    /// <summary> 
    /// 加密数据 
    /// </summary> 
    /// <param name="Text"></param> 
    /// <param name="sKey"></param> 
    /// <returns></returns> 
    public static string Encrypt(string Text, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        byte[] inputByteArray;
        inputByteArray = Encoding.Default.GetBytes(Text);
        des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        StringBuilder ret = new StringBuilder();
        foreach (byte b in ms.ToArray())
        {
            ret.AppendFormat("{0:X2}", b);
        }
        return ret.ToString();
    }

    #endregion

    #region ========解密========
    /// <summary>
    /// 解密
    /// </summary>
    /// <param name="Text"></param>
    /// <returns></returns>
    public static string DecryptDES(string Text)
    {
        return Decrypt(Text, "luyunhai");
    }
    /// <summary> 
    /// 解密数据 
    /// </summary> 
    /// <param name="Text"></param> 
    /// <param name="sKey"></param> 
    /// <returns></returns> 
    public static string Decrypt(string Text, string sKey)
    {
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        int len;
        len = Text.Length / 2;
        byte[] inputByteArray = new byte[len];
        int x, i;
        for (x = 0; x < len; x++)
        {
            i = Convert.ToInt32(Text.Substring(x * 2, 2), 16);
            inputByteArray[x] = (byte)i;
        }
        des.Key = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        des.IV = ASCIIEncoding.ASCII.GetBytes(System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(sKey, "md5").Substring(0, 8));
        System.IO.MemoryStream ms = new System.IO.MemoryStream();
        CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
        cs.Write(inputByteArray, 0, inputByteArray.Length);
        cs.FlushFinalBlock();
        return Encoding.Default.GetString(ms.ToArray());
    }

    #endregion
    #endregion
}