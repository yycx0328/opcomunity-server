using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Common
{
    public enum AppResource
    {
        [Remark("OP社应用")]
        OP = 1
    }

    /// <summary>
    /// 用户登录类型
    /// </summary>
    public enum UserAuthIdentityType
    {
        [Remark("手机")]
        phone = 1,
        [Remark("邮箱")]
        email = 2,
        [Remark("QQ登录")]
        qq = 3,
        [Remark("Sina登录")]
        sina = 4,
        [Remark("微信登录")]
        wechat = 5,
        [Remark("游客登录")]
        visitor = 6
    }

    /// <summary>
    /// 金币收支配置
    /// </summary>
    public enum CoinIOStatusConfig
    {
        [Remark("收入")]
        I,
        [Remark("支出")]
        O
    }

    /// <summary>
    /// 金币流水账配置，收入类型以2000开头，支出类型以4000开头
    /// </summary>
    public enum CoinJournalConfig:int
    {
        [Remark("活动奖励")]
        HandIn = 2000,
        [Remark("新用户赠送")]
        NewAccountDonate,
        [Remark("充值")]
        Charge,
        [Remark("通话收入")]
        LiveIncome,
        [Remark("收到礼物")]
        ReciveGift,
        [Remark("提现退还")]
        CashBack,
        [Remark("被邀请用户消费奖励")]
        InviteReward,
        [Remark("VIP每日赠送")]
        VIPPerDayDonate,

        [Remark("手动扣除")]
        HandOut = 4000,
        [Remark("提现")] 
        CashOut,
        [Remark("通话支出")]
        LiveOutcome,
        [Remark("发送礼物")]
        SendGift,
    }
     
    /// <summary>
    /// 金币收支配置
    /// </summary>
    public enum LiveCallSenderConfig
    {
        [Remark("用户")]
        User,
        [Remark("主播")]
        Anchor
    }

    public enum UploadImageCheckResult
    {
        [Remark("成功")]
        Success,
        [Remark("文件上传失败")]
        Failed,
        [Remark("文件格式错误，请上传gif、jpg、png格式的图片")]
        FileFormatterErr,
        [Remark("请上传gif、jpg、png格式的图片")]
        FileExtensionErr,
        [Remark("图片最大限制为0.5MB")]
        FileSizeErr,
        [Remark("图片最小尺寸为20*20，最大为480*854")]
        FileDimensionErr,
        [Remark("上传文件发生异常")]
        Excption,
    }

    public enum UploadVideoCheckResult
    {
        [Remark("成功")]
        Success,
        [Remark("文件上传失败")]
        Failed,
        [Remark("请上传MP4格式的视频")]
        FileExtensionErr,
        [Remark("视频封面截取失败")]
        VideoImageCatchErr,
        [Remark("上传文件发生异常")]
        Excption,
    }

    //public enum ChargeTypeConfig : int
    //{
    //    [Remark("Alipay")]
    //    AL,
    //    [Remark("Wechat Pay")]
    //    WC
    //}

    //public enum OrderStatusConfig : int
    //{
    //    [Remark("下单")]
    //    Init,
    //    [Remark("成功")]
    //    Success,
    //    [Remark("失败")]
    //    Failure
    //}

    public enum OrderCallbackStatusConfig : int
    {
        [Remark("初始化")]
        Init,
        [Remark("成功")]
        Success,
        [Remark("失败")]
        Failure
    }

    public enum OrderTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("订单签名失败")]
        OrderSignErr = 2000
    }

    public enum CheckResultTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("初始化错误")]
        InitErr = 2000,
        [Remark("注册失败")]
        RegistFaild,
        [Remark("验证码错误，请输入正确的验证码")]
        VerifyCodeErr,
        [Remark("验证码已失效，请重新获取验证码")]
        VerifyCodeAlreadyUseErr,
        [Remark("验证码已过期，请重新获取验证码")]
        VerifyCodeExpireErr,
        [Remark("无法识别的密码")]
        UnrecognizedPasswordErr,
        [Remark("该手机号已注册，请直接登录")]
        AlreadyRegistErr,
        [Remark("您的手机号已被冻结，请联系客服人员")]
        ForbiddenPhoneNoErr,
        [Remark("用户名或密码错误")]
        LoginAccountOrPasswordErr,
        [Remark("账号不存在，请前往注册")]
        AccountNoExistErr,
        [Remark("密码重置失败")]
        RestPasswordErr,
        [Remark("网易云通讯账号注册失败")]
        RegistNeteaseErr,
        [Remark("用户账号错误")]
        UserIdErr
    }

    public enum CheckLiveCallTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("主呼叫用户不存在")]
        UserNotExistErr = 2000,
        [Remark("主播账号不存在")]
        AnchorUserNotExistErr,
        [Remark("用户棉花糖数量不足")]
        UserCoinNotEnoughErr,
    }

    public enum ModifyAccountInfoTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("失败")]
        Failed = 2000,
        [Remark("用户不存在")]
        UserNotExistErr,
        [Remark("昵称与之前的一致，无法修改！")]
        NickNameSameAsOldName
    }

    public enum CashOutTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserAccountErr = 1000,
        [Remark("用户账号不存在")]
        UserNotExistErr = 2000,
        [Remark("账户收益金额不足")]
        UserCoinNotEnoughErr,
        [Remark("您不是主播账号，无法提现")]
        UserNotAnchorErr = 2000,
        [Remark("提现比率错误")]
        CashRatioErr,
    }

    public enum InviteTips : int
    {
        [Remark("成功")]
        Success = 0, 
        [Remark("用户账号不存在")]
        UserNotExistErr = 2000,
        [Remark("该手机号已被邀请")]
        PhoneHasBeenInvited, 
    }
}
