using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services
{
    public enum DiscoverCategoryConfig
    {
        Newest,
        Hottest
    }

    public enum UserCategoryConfig
    {
        Normal,
        Vip
    }

    public enum VideoListCategoryConfig
    {
        Hottest,
        Newest,
        Focus
    }

    public enum NeteaseLoginStatusConfig
    {
        [Remark("离线")]
        OffLine,
        [Remark("在线")]
        OnLine
    }

    public enum NeteaseChatStatusConfig
    {
        [Remark("离线")]
        OffLine,
        [Remark("在聊")]
        InChat,
        [Remark("空闲")]
        Free
    }

    public enum AnchorAuthStatusConfig
    {
        [Remark("未认证")]
        None,
        [Remark("认证中")]
        InAuth,
        [Remark("已认证")]
        Auth
    }

    public enum ProcessStatusConfig
    {
        [Remark("待处理")]
        WaitForProcess,
        [Remark("处理中")]
        InProcess,
        [Remark("已处理")]
        Processed,
        [Remark("忽略")]
        Ignore
    }

    public enum FollowTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("您已关注了该用户")]
        AlreadyFollowErr = 2000,
        [Remark("非主播账户，无法关注！")]
        NotAnchorAccountErr,
        [Remark("关注的用户不存在")]
        UserNotExistErr,
        [Remark("您尚未关注该用户，无须取消")]
        UnFollowErr,
        [Remark("无法关注自己")]
        CannotFollowSelfErr,
        [Remark("取消关注失败")]
        CancelFollowFaild,
        [Remark("关注失败")]
        FollowFaild
    }

    public enum CollectTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("您已收藏了该作品")]
        AlreadyCollectErr = 2000,
        [Remark("收藏的作品不存在")]
        TopicNotExistErr,
        [Remark("您尚未收藏该作品，无须取消")]
        UnCollectErr,
        [Remark("取消收藏失败")]
        CancelCollectFaild,
        [Remark("收藏失败")]
        CollectFaild
    }

    public enum LiveCallTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("只记录时长为0的通话记录")]
        OnlyRecordZeroTime,
        [Remark("通讯记录已存在")]
        LiveRecordAlreadyExist,
        [Remark("请重新登录")]
        UserNotExistErr = 1000,
        [Remark("非主播账号或主播账号不存在")]
        AnchorUserNotExistErr = 2000,
        [Remark("开始时间晚于结束时间")]
        StarttimeBigThanEndtimeErr,
        [Remark("时间重叠与已存在的通话记录重叠")]
        RepeatTimeErr,
        [Remark("不能自己呼叫自己本人")]
        CannotCallYourselfErr,
        [Remark("发送者参数异常")]
        SenderErr,
        [Remark("重复发送通讯记录")]
        RepeatSendLiveRecordErr
    }

    public enum DeleteLifeShowTips : int
    {
        [Remark("请求成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotExistErr = 1000,
        [Remark("图片不存在")]
        PictureNotExistErr = 2000,
        [Remark("无法删除他人的图片")]
        CannotDeleteOthersPictureErr
    }

    public enum AplayTobeAnchorTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("您己经是认证主播")]
        AlreadyAuthAnchorErr = 2000,
        [Remark("您的账号禁止申请为主播")]
        ForbiddenTobeAnchor
    }

    public enum SendGiftTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotExistErr = 1000,
        [Remark("您的账户余额不足")]
        UserCoinNotEnoughErr,
        [Remark("对方非主播账号")]
        AnchorUserNotExistErr = 2000,
        [Remark("礼物不存在")]
        GiftNotExistErr,
        [Remark("不能给自己发送礼物")]
        CannotSendGiftForYourselfErr,
    }

    public enum ExecuteStatusConfig : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("失败")]
        Failed,
        [Remark("初始化")]
        Init
    }

    public enum LiveCallSenderConfig : int
    {
        [Remark("用户")]
        User = 0,
        [Remark("主播")]
        Anchor
    }

    public enum MessageCategiryConfig : int
    {
        [Remark("消息")]
        Message = 0,
        [Remark("推送")]
        Push,
        [Remark("通知")]
        Notification
    }

    public enum CashStatusConfig : int
    {
        [Remark("提现中")]
        Cahing = 0,
        [Remark("处理中")]
        Processing,
        [Remark("已完成")]
        Finished,
        [Remark("退还")]
        Back,
        [Remark("扣压")]
        Withhold,
        [Remark("提现失败")]
        Failed
    }


    public enum OrderTypeConfig : int
    {
        #region 钻石充值
        [Remark("空类型")]
        None, 
        #endregion

        #region 会员充值
        [Remark("月会员")]
        Month,
        [Remark("季会员")]
        Quarter,
        [Remark("半年会员")]
        HalfYear,
        [Remark("年会员")]
        Year,
        #endregion

        #region 船票充值
        [Remark("无限制")]
        UnLimit,
        [Remark("限制10000")]
        Limit
        #endregion
    }

    //public enum CashOutTips : int
    //{
    //    [Remark("成功")]
    //    Success = 0,
    //    [Remark("请重新登录")]
    //    UserAccountErr = 1000,
    //    [Remark("您不是主播账号，无法提现")]
    //    UserNotAnchorErr = 2000,
    //    [Remark("提现比率错误")]
    //    CashRatioErr,
    //}

    public enum SetNeteaseLoginOrChatStatusTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("未查找到对应的网易云账号")]
        NeteaseAccountNotExistErr = 2000,
        [Remark("状态参数错误")]
        StatusParameterErr,
    }

    public enum SendMessageTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("失败")]
        Failed = 2000,
        [Remark("您已达到发送次数上限，成为VIP后畅享无限次聊天")]
        TimesLimitErr,
        [Remark("主播不存在")]
        AnchorNotExistErr,
        [Remark("您还不是VIP用户，请前往充值")]
        UserNotVipErr,
        [Remark("您还没有直达该主播的邮票，请前往充值")]
        UserNotTicketErr
    }

    public enum JPushSmsStatusConfig : int
    {
        [Remark("发送成功未启用")]
        Send = 0,
        [Remark("验证成功")]
        ValidSuccess,
        [Remark("验证失败")]
        ValidFailed
    }

    public enum LiveCallHistoryTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("初始化错误")]
        Init = 2000
    }

    public enum NeteaseCallNotifyTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("初始化错误")]
        Init = 1,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("非视频通话记录或聊天记录，无需保存")]
        EventTypeIsNotFive,
        [Remark("通话记录时长为零，无需记录")]
        LiveCallTimeIsZero,
        [Remark("重复发送通讯记录")]
        RepeatSendLiveRecord,
        [Remark("MD5或CheckSum验证错误")]
        MD5OrCheckSumErr = 2000,
        [Remark("您的账户余额不足")]
        UserCoinNotEnoughErr,
        [Remark("请求内容参数错误")]
        RequestBodyErr,
        [Remark("网易云账号不存在")]
        NeteaseAccountIsNotExistErr,
        [Remark("主播账号不存在")]
        AnchorAccountIsNotExistErr,
        [Remark("重复发送通讯记录")]
        RepeatSendLiveRecordErr,
        [Remark("通讯记录已存在")]
        LiveRecordAlreadyExistErr,
    }

    public enum ModifyAccountInfoTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("失败")]
        Failed = 2000,
        [Remark("用户不存在！")]
        UserNotExistErr,
        [Remark("昵称与之前的一致，无法修改！")]
        NickNameSameAsOldName
    }

    public enum CheckTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("初始化")]
        Init,
        [Remark("版本太低，请升级至最新版！")]
        VersionTooOld,
        [Remark("失败")]
        Failed = 2000
    }

    public enum ChargeTypeConfig : int
    {
        [Remark("Alipay")]
        AL,
        [Remark("WechatPay")]
        WX
    }

    public enum OrderStatusConfig : int
    {
        [Remark("下单")]
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
        OrderSignErr = 2000,
        [Remark("预付单生成失败")]
        WechatPrepayErr,
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
        [Remark("账号不存在，请去前往注册")]
        AccountNoExistErr,
        [Remark("密码重置失败")]
        RestPasswordErr,
        [Remark("网易云通讯账号注册失败")]
        RegistNeteaseErr,
        [Remark("用户账号错误")]
        UserIdErr
    }

    public enum BasicTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("初始化")]
        Init,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("失败")]
        Failed = 2000,
        [Remark("用户不存在！")]
        UserNotExistErr,
        [Remark("非主播账号")]
        UserNotAnchor,
        [Remark("参数错误")]
        ParameterErr
    }

    public enum SendBatchMessageTips : int
    {
        [Remark("成功")]
        Success = 0,
        [Remark("初始化")]
        Init,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("失败")]
        Failed = 2000,
        [Remark("用户不存在！")]
        UserNotExistErr,
        [Remark("非主播账号")]
        UserNotAnchor,
        [Remark("参数错误")]
        ParameterErr,
        [Remark("您发送太频繁，请于10分钟后再试")]
        TooFrequentlyErr,
        [Remark("无可送达用户")]
        NoneReceivedUserErr,
        [Remark("消息发送失败")]
        MessageSendFaildErr
    }

    public enum PraiseVideoTips : int
    {
        [Remark("点赞成功")]
        PraiseSuccess = 0,
        [Remark("取消点赞")]
        CancelPraiseSuccess = 1,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("视频已删除")]
        VideoNotExistsErr = 2000
    }

    public enum DeleteVideoTips : int
    {
        [Remark("删除成功")]
        Success = 0,
        [Remark("请重新登录")]
        UserNotLoginErr = 1000,
        [Remark("无法删除他人视频")]
        DeleteOthersVideoErr = 2000,
        [Remark("视频不存在")]
        VideoNotExistsErr
    }
}
