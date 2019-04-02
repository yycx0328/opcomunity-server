using Opcomunity.Passport.Entities;
using Utility.Common;

namespace Passport.Services.Interface
{
    public interface ILoginService
    {
        TB_ApplicationInfo GetApplicationInfo(string applicationId);
        
        /// <summary>
        /// 验证用户提交的验证码，且当用户不存在时，注册用户
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="code"></param>
        /// <param name="password">密码</param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        long PhoneRegistUserIfNotExist(string phoneNo, string password, string applicationId, out CheckResultTips tips);

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="code"></param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        long ResetPassword(string phoneNo, string password, out CheckResultTips tips);

        /// <summary>
        /// 根据账号密码获取用户信息
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        long GetPhoneLoginUserId(string phoneNo, string password);

        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //UserLoginModel GetUserInfo(long userId);

        ///// <summary>
        ///// 获取用户信息
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //UserLoginModel GetUserInfo(string phoneNo, string password);
    }
}
