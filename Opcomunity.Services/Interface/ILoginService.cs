using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using Utility.Common;

namespace Opcomunity.Services.Interface
{
    public interface ILoginService
    {
        long PhoneRegist(string phoneNo, string password, string applicationId, string nickName, string avatar,string thubnail,int channel, out CheckResultTips tips);

        long VisitorLogin(string uuid, string applicationId, string nickName, string avatar, string thumbnail, int channel, out CheckResultTips tips);

        TB_User GetPhoneLoginUserId(string phoneNo, string password);

        bool IsPhoneRegistOrInvited(string phoneNo);

        long ResetPassword(string phoneNo, string password, out CheckResultTips tips);

        /// <summary>
        /// 验证用户是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        TB_User GetUserById(long userId);

        /// <summary>
        /// 发送手机短信码
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        string SendMessageCode(string phoneNo);

        /// <summary>
        /// 存储用户基本信息
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <param name="code"></param>
        /// <param name="password">密码</param>
        /// <param name="applicationId"></param>
        /// <returns></returns>
        TB_User RegistPhoneUserInCurrentSystem(long userId, string phoneNo);

        /// <summary>
        /// 验证手机短信码
        /// </summary>
        /// <param name="messageId"></param>
        /// <returns></returns>
        bool ValidMessageCode(string phoneNo, string messageId, string code, out string errMessage);

        /// <summary>
        /// 存储用户登录成功后返回的User Token信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int UpdateUserToken(long userId);

        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        TB_User GetLoginUserInfo(long userId, string token);

        /// <summary>
        /// 获取用户登录信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        LoginUserDetailInfoItem GetLoginUserInfo(long userId);

        /// <summary>
        /// 验证账户是否为合法的登录账户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsLegalLoginUser(long userId, string token);

        /// <summary>
        /// 获取发送的话题数量
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetTopicCount(long userId);

        /// <summary>
        /// 获取用户关注数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        int GetUserFollowCount(long userId);
        
        /// <summary>
        /// 获取主播信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        TB_Anchor GetAnchor(long userId);

        /// <summary>
        /// 注册网易云通信账号
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        TB_NeteaseAccount GetNeteaseAccountIfNotExistThenRegist(long userId, string nickName, string avatar);

        /// <summary>
        /// 验证手机号是否被邀请过
        /// </summary>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        bool IsPhoneInvited(string phoneNo);

        /// <summary>
        /// 保存用户邀请信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phoneNo"></param>
        /// <returns></returns>
        InviteTips SaveInviteData(long userId, string phoneNo);
    }
}
