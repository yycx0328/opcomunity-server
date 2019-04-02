using log4net;
using Opcomunity.Passport.Entities;
using Passport.Services.Interface;
using System;
using System.Linq;
using System.Reflection;
using Utility.Common;

namespace Passport.Services.Implementations
{
    public class LoginService : ServiceBase, ILoginService
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public long PhoneRegistUserIfNotExist(string phoneNo, string password, string applicationId,out CheckResultTips tips)
        {
            using (var context = base.NewContext())
            {
                long userId = 0;
                var userModel = context.TB_UserAuth.FirstOrDefault(p => 
                    p.IdentityType == UserAuthIdentityType.phone.ToString()
                    && p.Identifier == phoneNo);
                
                if (userModel != null)
                {
                    tips = CheckResultTips.AlreadyRegistErr;
                    return 0;
                }

                // 验证码验证成功同时账号未注册过，则自动注册新账号
                #region 验证码验证成功同时账号未注册过，则自动注册新账号
                TB_User user = new TB_User()
                {
                    RegistApplication = applicationId,
                    IsLegal = true,
                    CreateTime = DateTime.Now,
                    UpdateTime = DateTime.Now
                };
                // 新增用户
                context.TB_User.Add(user);
                context.SaveChanges();
                // 返回用户Id
                userId = user.Id;

                TB_UserAuth userAuth = new TB_UserAuth()
                {
                    UserId = user.Id,
                    IdentityType = UserAuthIdentityType.phone.ToString(),
                    Identifier = phoneNo,
                    Credential = password,
                    Ip = WebUtils.GetClientIP(),
                    FirstLoginApp = applicationId,
                    IsLegal = true,
                    CreateTime = DateTime.Now,
                    LastLoginTime = DateTime.Now
                }; 
                context.TB_UserAuth.Add(userAuth);
                context.SaveChanges();
                #endregion
                tips = CheckResultTips.Success;
                return userId;
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

        /// <summary>
        /// 根据账号密码获取用户信息
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public long GetPhoneLoginUserId(string phoneNo, string password)
        {
            if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(password))
                return 0;
            using (var context = base.NewContext())
            {
                var query = from identify in context.TB_UserAuth
                            join info in context.TB_User
                            on identify.UserId equals info.Id
                            where identify.IsLegal && identify.IdentityType == UserAuthIdentityType.phone.ToString()
                            && identify.Identifier == phoneNo && identify.Credential == password
                            select info.Id;
                return query.SingleOrDefault();
            }
        }

        ///// <summary>
        ///// 根据账号密码获取用户信息
        ///// </summary>
        ///// <param name="phoneNo"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public UserLoginModel GetUserInfo(string phoneNo, string password)
        //{
        //    if (string.IsNullOrEmpty(phoneNo) || string.IsNullOrEmpty(password))
        //        return null;
        //    using (var context = base.NewContext())
        //    {
        //        var query = from identify in context.TB_UserAuth
        //                    join info in context.TB_User
        //                    on identify.UserId equals info.Id
        //                    where identify.IsLegal && identify.IdentityType == UserAuthIdentityType.phone.ToString()
        //                    && identify.Identifier == phoneNo && identify.Credential == password
        //                    select new UserLoginModel {
        //                        UserId = info.Id,
        //                        NickName = info.NickName,
        //                        Avatar = info.Avatar,
        //                        PhoneNo = info.PhoneNo,
        //                        WeChat = info.WeChat,
        //                        QQ = info.QQ,
        //                        Height = info.Height,
        //                        Weight = info.Weight,
        //                        Birthday = info.Birthday,
        //                        Constellation = info.Constellation
        //                    };
        //        return query.FirstOrDefault();
        //    }
        //}

        ///// <summary>
        ///// 根据用户Id获取用户信息
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public UserLoginModel GetUserInfo(long userId)
        //{
        //    if (userId <= 0)
        //        return null;
        //    using (var context = base.NewContext())
        //    {
        //        var user = context.TB_User.SingleOrDefault(p => p.Id == userId);
        //        if (user == null)
        //            return null;
        //        var model = new UserLoginModel()
        //        {
        //            UserId = user.Id,
        //            NickName = user.NickName,
        //            Avatar = user.Avatar,
        //            PhoneNo = user.PhoneNo,
        //            WeChat = user.WeChat,
        //            QQ = user.QQ,
        //            Height = user.Height,
        //            Weight = user.Weight,
        //            Birthday = user.Birthday,
        //            Constellation = user.Constellation
        //        };
        //        return model;
        //    }
        //}

        public TB_ApplicationInfo GetApplicationInfo(string applicationId)
        {
            using (var context = base.NewContext())
            { 
                return context.TB_ApplicationInfo.FirstOrDefault(p => p.ApplicationId == applicationId);
            }
        } 
    }
}
