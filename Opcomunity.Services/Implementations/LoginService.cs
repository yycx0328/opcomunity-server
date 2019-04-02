using System;
using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using System.Linq;
using System.Collections.Specialized;
using Opcomunity.Services.Helpers;
using log4net;
using System.Reflection;
using Newtonsoft.Json.Linq;
using Utility.Common;
using Opcomunity.Services.Dtos;
using System.Data.Entity;

namespace Opcomunity.Services.Implementations
{
    public class LoginService : ServiceBase, ILoginService
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        //private static readonly int defaultCashRatio = TypeHelper.TryParse(ConfigurationManager.AppSettings["DefaultCashRatio"], 50);

        public long PhoneRegist(string phoneNo, string password, string applicationId,string nickName, string avatar, string thumbnail, int channel, out CheckResultTips tips)
        {
            using (var context = base.NewContext())
            {
                long userId = 0;
                #region 验证手机号是否已注册
                var userModel = context.TB_UserAuth.FirstOrDefault(p =>
                            p.IdentityType == UserAuthIdentityType.phone.ToString()
                            && p.Identifier == phoneNo);
                if (userModel != null)
                {
                    tips = CheckResultTips.AlreadyRegistErr;
                    return 0;
                }
                #endregion

                #region 生成并保存用户基本信息
                // 保存用户基本信息
                var user = new TB_User()
                { 
                    NickName = nickName,
                    Avatar = avatar,
                    ThumbnailAvatar = thumbnail,
                    Description = "一通电话，连通你我！",
                    PhoneNo = phoneNo
                };
                // 新增用户
                context.TB_User.Add(user);
                context.SaveChanges();
                // 返回用户Id
                userId = user.Id;
                Log4NetHelper.Info(log, "UserId:"+userId);
                #endregion
                 
                #region 新增用户账号绑定信息
                TB_UserAuth userAuth = new TB_UserAuth()
                {
                    UserId = user.Id,
                    IdentityType = UserAuthIdentityType.phone.ToString(),
                    Identifier = phoneNo,
                    Credential = password,
                    Ip = WebUtils.GetClientIP(),
                    FirstLoginApp = applicationId,
                    IsLegal = true,
                    Channel = channel,
                    CreateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };
                context.TB_UserAuth.Add(userAuth);
                #endregion

                #region 新增Token信息
                TB_UserTokenInfo model = new TB_UserTokenInfo()
                {
                    UserId = userId,
                    UserToken = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddDays(30)
                };
                context.TB_UserTokenInfo.Add(model);
                #endregion

                var inviteUserModel = context.TB_UserInvite.SingleOrDefault(p => p.PhoneNo == phoneNo);
                if (inviteUserModel != null)
                    inviteUserModel.NewUserId = userId;

                #region 赠送活动金额
                int donateCoin = ConfigHelper.GetValue("DONATECOIN",0);
                if (donateCoin > 0)
                {
                    // 活动赠送金币
                    TB_UserCoin userCoin = new TB_UserCoin()
                    {
                        UserId = userId,
                        CurrentCoin = donateCoin,
                        CurrentIncome = 0
                    };
                    context.TB_UserCoin.Add(userCoin);

                    // 用户金币交易记录
                    TB_UserCoinJournal coinJournal = new TB_UserCoinJournal()
                    {
                        UserId = userId,
                        CoinCount = donateCoin,
                        CurrentCount = donateCoin,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.NewAccountDonate,
                        JournalDesc = CoinJournalConfig.NewAccountDonate.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserCoinJournal.Add(coinJournal);
                } 
                #endregion

                context.SaveChanges();
                tips = CheckResultTips.Success;
                return userId;
            }
        }
         
        /// <summary>
        /// 根据账号密码获取用户信息
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public TB_User GetPhoneLoginUserId(string phoneNo, string password)
        {
            if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(password))
                return null;
            using (var context = base.NewContext())
            {
                var query = from identify in context.TB_UserAuth
                            join info in context.TB_User
                            on identify.UserId equals info.Id
                            where identify.IsLegal && identify.IdentityType == UserAuthIdentityType.phone.ToString()
                            && identify.Identifier == phoneNo && identify.Credential == password
                            select info;
                return query.SingleOrDefault();
            }
        }

        public bool IsPhoneRegistOrInvited(string phoneNo)
        {
            using (var context = base.NewContext())
            {
                var isRegist = context.TB_UserAuth.SingleOrDefault(p =>
                    p.IdentityType == UserAuthIdentityType.phone.ToString()
                    && p.Identifier == phoneNo)!= null;
                var isInvited = context.TB_UserInvite.SingleOrDefault(p => p.PhoneNo == phoneNo)!=null;
                if (isRegist || isInvited)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="code"></param>
        /// <param name="password"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public long ResetPassword(string phoneNo, string password, out CheckResultTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from ua in context.TB_UserAuth
                            where ua.IdentityType == UserAuthIdentityType.phone.ToString()
                            && ua.Identifier == phoneNo
                            select ua;
                var userModel = query.SingleOrDefault();
                if (userModel == null)
                {
                    tips = CheckResultTips.AccountNoExistErr;
                    return 0;
                }

                if (!userModel.IsLegal)
                {
                    tips = CheckResultTips.ForbiddenPhoneNoErr;
                    return 0;
                }

                // 验证码验证成功同时账号未注册过，则自动注册新账号
                #region 验证码验证成功同时账号未注册过，则自动注册新账号
                userModel.Credential = password;
                context.SaveChanges();
                tips = CheckResultTips.Success;
                return userModel.UserId;
                #endregion
            }
        }

        public TB_User GetUserById(long userId)
        {
            using (var context = base.NewContext())
            {
                return context.TB_User.SingleOrDefault(p => p.Id == userId);
            }
        }

        public int UpdateUserToken(long userId)
        {
            using (var context = base.NewContext())
            {
                var model = context.TB_UserTokenInfo.FirstOrDefault(p => p.UserId == userId);
                if (model != null)
                {
                    model.UserToken = Guid.NewGuid().ToString();
                    model.ExpireTime = DateTime.Now.AddDays(30);
                }
                else
                {
                    model = new TB_UserTokenInfo()
                    {
                        UserId = userId,
                        UserToken = Guid.NewGuid().ToString(),
                        ExpireTime = DateTime.Now.AddDays(30),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserTokenInfo.Add(model);
                }
                return context.SaveChanges();
            }
        }

        public bool IsLegalLoginUser(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo.Id;
                if (query.Count() >= 0)
                    return true;
                return false;
            }
        }
        
        public int GetUserFollowCount(long userId)
        {
            using (var context = base.NewContext())
            {
                var query = from follow in context.TB_UserFollow
                        where follow.FollowedUserId == userId
                        select follow;
                return query.Count();
            }
        }

        public int GetTopicCount(long userId)
        {
            using (var context = base.NewContext())
            {
                var query = from topic in context.TB_Topic
                            where topic.UserId == userId
                            select topic;
                return query.Count();
            }
        }
        
        public TB_Anchor GetAnchor(long userId)
        {
            using (var context = base.NewContext())
            {
                return context.TB_Anchor.SingleOrDefault(p => p.UserId == userId && p.IsAuth && !p.IsBlack);
            }
        }

        public TB_NeteaseAccount GetNeteaseAccountIfNotExistThenRegist(long userId, string nickName, string avatar)
        {
            if (userId <=0)
                return null;

            using (var context = base.NewContext())
            {
                var neteaseAccount = context.TB_NeteaseAccount.SingleOrDefault(p => p.UserId == userId);
                if (neteaseAccount != null)
                {
                    neteaseAccount.UpdateTime = DateTime.Now;
                    context.SaveChanges();
                    return neteaseAccount;
                }

                string accId = WebUtils.MD5(string.Format("{0}{1}{2}", userId,
                    ConfigHelper.GetValue("NETEASE_ACCID_MD5_KEY"),DateTime.Now.ToString("yyyyMMddHHmmss"))).ToLower();
                NameValueCollection data = new NameValueCollection();
                data.Add("accid", accId);
                data.Add("name", nickName);
                data.Add("icon", avatar);
                string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.CRT_USER, data);
                Log4NetHelper.Info(log, result);
                if (string.IsNullOrEmpty(result))
                    return null;

                JObject jobject = JObject.Parse(result);
                if (jobject == null || jobject["code"].ToString() != "200")
                    return null;

                var info = JSONSerializeUtil.ToObject<NeteaseAccountInfo>(jobject["info"].ToString());
                if (info == null)
                    return null;

                var model = new TB_NeteaseAccount
                {
                    UserId = userId,
                    NeteaseAccId = info.accid,
                    NeteaseToken = info.token,
                    IsAvailable = true,
                    LoginStatus = (int)NeteaseLoginStatusConfig.OnLine,
                    ChatStatus = (int)NeteaseChatStatusConfig.Free,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                
                context.TB_NeteaseAccount.Add(model);
                context.SaveChanges();
                return model;
            }
        }

        public TB_User GetLoginUserInfo(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                var user = query.SingleOrDefault();
                if (user != null)
                    return user;
                return null;
            }
        }
         
        public LoginUserDetailInfoItem GetLoginUserInfo(long userId)
        {
            using (var context = base.NewContext())
            {
                var unixTime = new DateTime(1970, 1, 1, 8, 0, 0);
                var defaultCashRatio = ConfigHelper.GetValue("DefaultCashRatio", 50);
                var query = from u in context.TB_User
                            join t in context.TB_UserTokenInfo
                            on u.Id equals t.UserId
                            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                            on u.Id equals a.UserId
                            into ua
                            from a in ua.DefaultIfEmpty()
                            join n in context.TB_NeteaseAccount
                            on u.Id equals n.UserId
                            into un
                            from n in un.DefaultIfEmpty()
                            join c in context.TB_UserCoin
                            on u.Id equals c.UserId
                            into uc
                            from c in uc.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on u.Id equals v.UserId
                            into uv
                            from v in uv.DefaultIfEmpty()
                            where u.Id == userId
                            select new LoginUserDetailInfoItem
                            {
                                UserId = u.Id,
                                Token = t.UserToken,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Birthday = u.Birthday,
                                Height = u.Height,
                                Weight = u.Weight,
                                PhoneNo = u.PhoneNo,
                                WeChat = u.WeChat,
                                QQ = u.QQ,
                                Constellation = u.Constellation,
                                CurrentCoin = c == null ? 0 : c.CurrentCoin,
                                CurrentIncome = c == null ? 0 : c.CurrentIncome,
                                IsAnchor = a != null,
                                AuthStatus = a == null ? (int)AnchorAuthStatusConfig.None : a.IsAuth ? (int)AnchorAuthStatusConfig.Auth : (int)AnchorAuthStatusConfig.InAuth,
                                CallRatio = a == null ? 0 : a.CallRatio,
                                CashRatio = a == null ? defaultCashRatio : a.CashRatio,
                                Glamour = a == null ? 0 : a.Glamour,
                                FollowCount = (from f in context.TB_UserFollow
                                                 where f.UserId == userId
                                                 select f).Count(),
                                FollowedCount = (from f in context.TB_UserFollow
                                                   where f.FollowedUserId == userId
                                                   select f).Count(),
                                NeteaseAccId = n== null ? "": n.NeteaseAccId,
                                NeteaseToken = n == null ? "" : n.NeteaseToken,
                                NeteaseLoginStatus = n == null ? 0 : n.LoginStatus,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                IsVip = v != null,
                                VipExpireTime = v == null ? 0 : DbFunctions.DiffSeconds(unixTime, v.EndTime) ?? 0,
                                UserPhotoItems = from up in context.TB_UserPhoto
                                                 where up.UserId == userId
                                                 orderby up.CreateTime descending
                                                 select new UserPhotoItem
                                                 {
                                                     Id = up.Id,
                                                     ImageWebPath = up.ImageWebPath,
                                                     ThumbnailPath = up.ThumbnailPath
                                                 }
                            };
                return query.SingleOrDefault();
            }
        }

        public string SendMessageCode(string phoneNo)
        { 
            int templateId = ConfigHelper.GetValue("JPushTemplateId",1); 
            string result = JPushCore.SendCode(phoneNo, templateId);
            Log4NetHelper.Info(log, result);
            if (string.IsNullOrEmpty(result))
                return string.Empty;

            //Json数据：成功：{"msg_id": "288193860302"}；失败：{ "error": { "code": *****, "message": "******" }}
            if (result.IndexOf("error") >= 0)
                return string.Empty;

            JObject jobject = JObject.Parse(result);
            string messageId = "";
            if (jobject != null)
                messageId = jobject["msg_id"].ToString();

            if (string.IsNullOrEmpty(messageId))
                return string.Empty;

            using (var context = base.NewContext())
            {
                var model = new TB_JPushSms
                {
                    PhoneNo = phoneNo,
                    TemplateId = templateId,
                    MessageId = messageId,
                    Status = 0,
                    StatusDescription = "发送成功未启用",
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_JPushSms.Add(model);
                context.SaveChanges();
                return messageId;
            }
        }

        public bool ValidMessageCode(string phoneNo, string messageId, string code, out string errMessage)
        {
            using (var context = base.NewContext())
            {
                errMessage = "";

                var model = context.TB_JPushSms.FirstOrDefault(p => p.PhoneNo ==phoneNo && p.MessageId == messageId && p.Status == 0);
                if (model == null)
                {
                    errMessage = "验证码已失效，请重新获取！";
                    return false;
                }
                
                string result = JPushCore.ValidateCode(messageId, code);
                Log4NetHelper.Info(log, result);
                if (string.IsNullOrEmpty(result))
                {
                    errMessage = "请求验证失败！";
                    return false;
                }

                //Json数据：成功：{"is_valid": true}；失败：{"is_valid": false,"error": {"code": *****,"message": "******"}}
                JObject jobject = JObject.Parse(result);
                if (jobject["is_valid"].ToString() == "false")
                {
                    JObject jError = JObject.Parse(jobject["error"].ToString());
                    errMessage = jError["message"].ToString();
                    model.Code = code;
                    model.Status = (int)JPushSmsStatusConfig.ValidFailed;
                    model.StatusDescription = JPushSmsStatusConfig.ValidFailed.GetRemark();
                    context.SaveChanges();
                    return false;
                }

                model.Code = code;
                model.Status = (int)JPushSmsStatusConfig.ValidSuccess;
                model.StatusDescription = JPushSmsStatusConfig.ValidSuccess.GetRemark();
                context.SaveChanges();
                return true;
            }
        }
        
        public TB_User RegistPhoneUserInCurrentSystem(long userId,string phoneNo)
        {
            using (var context = base.NewContext())
            {
                // 保存用户基本信息
                var user = new TB_User()
                {
                    Id = userId,
                    NickName = Tools.GetDefaultNickName("MLQ-", 8),
                    Avatar = Tools.GetDefaultAvatar(),
                    ThumbnailAvatar = Tools.GetDefaultThumbnailAvatar(),
                    Description = "一通电话，连通你我！",
                    PhoneNo = phoneNo
                }; 
                context.TB_User.Add(user);
                 
                TB_UserTokenInfo model = new TB_UserTokenInfo()
                {
                    UserId = userId,
                    UserToken = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddDays(30)
                };
                context.TB_UserTokenInfo.Add(model);

                int donateCoin = ConfigHelper.GetValue("DONATECOIN", 0);
                if (donateCoin>0)
                {
                    // 活动赠送金币
                    TB_UserCoin userCoin = new TB_UserCoin()
                    {
                        UserId = userId,
                        CurrentCoin = donateCoin,
                        CurrentIncome = 0
                    };
                    context.TB_UserCoin.Add(userCoin);

                    // 用户金币交易记录
                    TB_UserCoinJournal coinJournal = new TB_UserCoinJournal()
                    {
                        UserId = userId,
                        CoinCount = donateCoin,
                        CurrentCount = donateCoin,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.NewAccountDonate,
                        JournalDesc = CoinJournalConfig.NewAccountDonate.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserCoinJournal.Add(coinJournal);
                }
                context.SaveChanges(); 
                return user;
            }
        }

        public bool IsPhoneInvited(string phoneNo)
        {
            using (var context = base.NewContext())
            {
                return context.TB_UserInvite.FirstOrDefault(p => p.PhoneNo == phoneNo) != null;
            }
        }

        public InviteTips SaveInviteData(long userId, string phoneNo)
        {
            using (var context = base.NewContext()) {
                var user = context.TB_User.SingleOrDefault(p => p.Id == userId);
                if (user == null)
                    return InviteTips.UserNotExistErr;

                var model = context.TB_UserInvite.SingleOrDefault(p => p.PhoneNo == phoneNo);
                if (model != null)
                    return InviteTips.PhoneHasBeenInvited;

                int costAwardRate = ConfigHelper.GetValue("AWARDRATE", 10);
                int cashAwardRate = costAwardRate; 
                model = new TB_UserInvite() {
                    UserId = userId,
                    PhoneNo = phoneNo,
                    CostAwardRate = costAwardRate,
                    CashoutAwardRate = cashAwardRate,
                    Ip = WebUtils.GetClientIP(),
                    InviteTime = DateTime.Now
                };
                context.TB_UserInvite.Add(model);
                context.SaveChanges();
                return InviteTips.Success;
            }
        }

        public long VisitorLogin(string uuid, string applicationId, string nickName, string avatar, string thumbnail, int channel, out CheckResultTips tips)
        {
            using (var context = base.NewContext())
            {
                long userId = 0;
                #region 验证手机号是否已注册
                var userModel = context.TB_UserAuth.FirstOrDefault(p =>
                            p.IdentityType == UserAuthIdentityType.visitor.ToString()
                            && p.Identifier == uuid);
                if (userModel != null)
                {
                    tips = CheckResultTips.AlreadyRegistErr;
                    return userModel.UserId;
                }
                #endregion

                #region 生成并保存用户基本信息
                // 保存用户基本信息
                var user = new TB_User()
                {
                    NickName = nickName,
                    Avatar = avatar,
                    ThumbnailAvatar = thumbnail,
                    Description = "一通电话，连通你我！",
                    PhoneNo = ""
                };
                // 新增用户
                context.TB_User.Add(user);
                context.SaveChanges();
                // 返回用户Id
                userId = user.Id;
                Log4NetHelper.Info(log, "UserId:" + userId);
                #endregion

                #region 新增用户账号绑定信息
                TB_UserAuth userAuth = new TB_UserAuth()
                {
                    UserId = user.Id,
                    IdentityType = UserAuthIdentityType.visitor.ToString(),
                    Identifier = uuid,
                    Credential = "",
                    Ip = WebUtils.GetClientIP(),
                    FirstLoginApp = applicationId,
                    IsLegal = true,
                    Channel = channel,
                    CreateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                };
                context.TB_UserAuth.Add(userAuth);
                #endregion

                #region 新增Token信息
                TB_UserTokenInfo model = new TB_UserTokenInfo()
                {
                    UserId = userId,
                    UserToken = Guid.NewGuid().ToString(),
                    CreateTime = DateTime.Now,
                    ExpireTime = DateTime.Now.AddDays(30)
                };
                context.TB_UserTokenInfo.Add(model);
                #endregion
                
                #region 赠送活动金额
                int donateCoin = ConfigHelper.GetValue("DONATECOIN", 0);
                if (donateCoin > 0)
                {
                    // 活动赠送金币
                    TB_UserCoin userCoin = new TB_UserCoin()
                    {
                        UserId = userId,
                        CurrentCoin = donateCoin,
                        CurrentIncome = 0
                    };
                    context.TB_UserCoin.Add(userCoin);

                    // 用户金币交易记录
                    TB_UserCoinJournal coinJournal = new TB_UserCoinJournal()
                    {
                        UserId = userId,
                        CoinCount = donateCoin,
                        CurrentCount = donateCoin,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.NewAccountDonate,
                        JournalDesc = CoinJournalConfig.NewAccountDonate.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserCoinJournal.Add(coinJournal);
                }
                #endregion

                context.SaveChanges();
                tips = CheckResultTips.Success;
                return userId;
            }
        }
    }
}
