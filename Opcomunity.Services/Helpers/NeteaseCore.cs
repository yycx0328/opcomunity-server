using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Helpers
{
    public enum NeteaseRequestActionConfig
    {
        CRT_USER,
        CRT_UPDATEUSER_URL,
        UPD_USERID,
        UPDGET_USERTOKEN,
        BLOCK_USERID,
        UNBLOCK_USERID,
        SEND_BATCH_MSG,
        SEND_MSG
    }

    public class NeteaseCore
    {
        #region 服务器操作URL
        // 创建【网易云通信ID】的Url
        //  参数	    类型    必须	    说明
        //  accid	String	是	    网易云通信ID，最大长度32字符，必须保证一个APP内唯一（只允许字母、数字、半角下划线_、@、半角点以及半角-组成，不区分大小写，会统一小写处理，请注意以此接口返回结果中的accid为准）。
        //  name	String	否	    网易云通信ID昵称，最大长度64字符，用来PUSH推送时显示的昵称
        //  props	String	否	    json属性，第三方可选填，最大长度1024字符
        //  icon	String	否	    网易云通信ID头像URL，第三方可选填，最大长度1024
        //  token	String	否	    网易云通信ID可以指定登录token值，最大长度128字符，并更新，如果未指定，会自动生成token，并在创建成功后返回
        //  sign	String	否	    用户签名，最大长度256字符
        //  email	String	否	    用户email，最大长度64字符
        //  birth	String	否	    用户生日，最大长度16字符
        //  mobile	String	否	    用户mobile，最大长度32字符，只支持国内号码
        //  gender	int	    否	    用户性别，0表示未知，1表示男，2女表示女，其它会报参数错误
        //  ex	    String	否	    用户名片扩展字段，最大长度1024字符，用户可自行扩展，建议封装成JSON字符串
        static string CRT_USER_URL = "https://api.netease.im/nimserver/user/create.action";

        /// <summary>
        /// 修改【网易云通信】
        /// accid	String	是	    网易云通信ID，最大长度32字符，必须保证一个APP内唯一（只允许字母、数字、半角下划线_、@、半角点以及半角-组成，不区分大小写，会统一小写处理，请注意以此接口返回结果中的accid为准）。
        //  name	String	否	    网易云通信ID昵称，最大长度64字符，用来PUSH推送时显示的昵称
        //  props	String	否	    json属性，第三方可选填，最大长度1024字符
        //  icon	String	否	    网易云通信ID头像URL，第三方可选填，最大长度1024
        //  token	String	否	    网易云通信ID可以指定登录token值，最大长度128字符，并更新，如果未指定，会自动生成token，并在创建成功后返回
        /// </summary>
        static string CRT_UPDATEUSER_URL = "https://api.netease.im/nimserver/user/updateUinfo.action";

        // 更新【网易云通信ID】的Url 非修改用户信息用
        // 参数	    类型	   必须	   说明
        // accid	String	是	   网易云通信ID，最大长度32字符，必须保证一个APP内唯一
        // props	String	否	   json属性，第三方可选填，最大长度1024字符
        // token	String	否	   网易云通信ID可以指定登录token值，最大长度128字符
        static string UPD_USERID_URL = "https://api.netease.im/nimserver/user/update.action";

        // 更新并获取新token
        // 参数	    类型	   必须	   说明
        // accid	String	是	   网易云通信ID，最大长度32字符，必须保证一个APP内唯一
        static string UPDGET_USERTOKEN_URL = "https://api.netease.im/nimserver/user/refreshToken.action";

        // 封禁网易云通信ID
        // 参数	    类型	   必须	   说明
        // accid	String	是	   网易云通信ID，最大长度32字符，必须保证一个APP内唯一
        // needkick	String	否	   是否踢掉被禁用户，true或false，默认false
        static string BLOCK_USERID_URL = "https://api.netease.im/nimserver/user/block.action";

        // 解禁网易云通信ID
        // 参数	    类型	   必须	   说明
        // accid	String	是	   网易云通信ID，最大长度32字符，必须保证一个APP内唯一
        static string UNBLOCK_USERID_URL = "https://api.netease.im/nimserver/user/unblock.action";

        /// <summary>
        /// 批量发送点对点普通消息
        /// 1.给用户发送点对点普通消息，包括文本，图片，语音，视频，地理位置和自定义消息。
        /// 2.最大限500人，只能针对个人,如果批量提供的帐号中有未注册的帐号，会提示并返回给用户。
        /// 3.此接口受频率控制，一个应用一分钟最多调用120次，超过会返回416状态码，并且被屏蔽一段时间；
        /// </summary>
        static string SEND_BATCH_MSG = "https://api.netease.im/nimserver/msg/sendBatchMsg.action";

        /// <summary>
        /// 给用户或者高级群发送普通消息，包括文本，图片，语音，视频和地理位置
        /// </summary>
        static string SEND_MSG_URL = "https://api.netease.im/nimserver/msg/sendMsg.action";
        #endregion 服务器操作URL

        //云信AppKey 公匙
        static string appKey = ConfigHelper.GetValue("NETEASE_APP_KEY");
        //云信AppSecret 私钥  
        static string appSecret = ConfigHelper.GetValue("NETEASE_APP_SECRET");

        /// <summary>
        /// 执行请求
        /// </summary>
        /// <param name="action">
        /// 操作码 1创建用户 2更新用户ID 3更新获取用户token 4禁用用户 5解禁用户
        /// </param>
        /// <param name="reqParams">参数 key=value</param>
        /// <returns></returns>
        public static string PostNeteaseRequest(NeteaseRequestActionConfig action, NameValueCollection data)
        {
            string url = string.Empty;
            switch (action)
            {
                case NeteaseRequestActionConfig.CRT_USER://创建用户
                    url = CRT_USER_URL;
                    break;
                case NeteaseRequestActionConfig.CRT_UPDATEUSER_URL:
                    url = CRT_UPDATEUSER_URL;
                    break;
                case NeteaseRequestActionConfig.UPD_USERID://更新用户ID
                    url = UPD_USERID_URL;
                    break;
                case NeteaseRequestActionConfig.UPDGET_USERTOKEN://更新并获取新的Token
                    url = UPDGET_USERTOKEN_URL;
                    break;
                case NeteaseRequestActionConfig.BLOCK_USERID://封禁网易云通信ID
                    url = BLOCK_USERID_URL;
                    break;
                case NeteaseRequestActionConfig.UNBLOCK_USERID://解禁被封禁的网易云通信ID
                    url = UNBLOCK_USERID_URL;
                    break;
                case NeteaseRequestActionConfig.SEND_BATCH_MSG:
                    url = SEND_BATCH_MSG;
                    break;
                case NeteaseRequestActionConfig.SEND_MSG:
                    url = SEND_MSG_URL;
                    break;
                default:
                    return "{\"desc\":\"方法选择错误!\",\"code\":-1}";
            }
            WebRequest wReq = WebRequest.Create(url);
            // 设定云信的相关校验值
            wReq.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
            wReq.Method = "POST";
            wReq.Headers.Add("AppKey", appKey);
            // 随机数（最大长度128个字符）
            string nonce = new Random().Next(100000, 999999).ToString();
            wReq.Headers.Add("Nonce", nonce);
            // 当前UTC时间戳，从1970年1月1日0点0 分0 秒开始到现在的秒数(String)
            string curTime = ((DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            wReq.Headers.Add("CurTime", curTime);
            // SHA1(AppSecret + Nonce + CurTime),三个参数拼接的字符串，
            // 进行SHA1哈希计算，转化成16进制字符(String，小写)
            wReq.Headers.Add("CheckSum", SHA1Hash(appSecret + nonce + curTime));

            // 传递相关操作参数
            string reqParams = SplicePostParameters(data);
            byte[] btBodys = Encoding.UTF8.GetBytes(reqParams);
            wReq.ContentLength = btBodys.Length;
            using (var wsr = wReq.GetRequestStream())
            {
                wsr.Write(btBodys, 0, btBodys.Length);
            }
            // 传递相关操作参数

            WebResponse wResp = wReq.GetResponse();
            Stream respStream = wResp.GetResponseStream();

            string resultJson;
            using (StreamReader reader = new StreamReader(respStream, Encoding.UTF8))
            {
                resultJson = reader.ReadToEnd();
            }
            //Json数据：desc描述，code状态码
            return resultJson;
        }

        /// <summary>
        /// 验证消息抄送提交的checksum和md5值
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="curTime"></param>
        /// <param name="checkSum"></param>
        /// <param name="requestBody"></param>
        /// <returns></returns>
        public static bool CheckNotifyRequest(string md5,string curTime, string checkSum, string requestBody)
        {
            if (md5 .ToLower()!= WebUtils.MD5(requestBody).ToLower())
                return false;
            if (checkSum.ToLower() != SHA1Hash(appSecret + md5 + curTime).ToLower())
                return false;
            return true;
        }

        private static string SplicePostParameters(NameValueCollection data)
        {
            string parameters = "";
            for (int i = 0; i < data.Count; i++)
            {
                if (i > 0)
                    parameters += string.Format("&{0}={1}", data.Keys[i], data[i]);
                else
                    parameters += string.Format("{0}={1}", data.Keys[i], data[i]);
            }
            return parameters;
        }

        //【工具】计算SHA1值
        private static string SHA1Hash(string input)
        {
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] bytes_sha1_in = UTF8Encoding.Default.GetBytes(input);
            byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
            string output = BitConverter.ToString(bytes_sha1_out);
            output = output.Replace("-", "").ToLower();
            return output;
        }
    }
}