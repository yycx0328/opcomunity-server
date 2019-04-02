using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Common;

namespace Opcomunity.Services.Interface
{
    public interface IAccountService
    {
        /// <summary>
        /// 验证是否为登录用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsLoginUser(long userId, string token);
          
        /// <summary>
        /// 获取用户个人详情
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        LoginUserDetailInfoItem GetUserDetailInfo(long userId, string token, out CheckTips tips);

        TB_UserCoin GetCoinCount(long userId, string token, out BasicTips tips);

        UserDetailInfoItem GetOthersDetailInfo(long userId, out CheckTips tips);

        UserDetailInfoItem GetUserInfoByAccId(string accId);

        /// <summary>
        /// 保存用户的生活照
        /// </summary>
        /// <param name="photos"></param>
        /// <returns></returns>
        bool SaveLifeShowImages(List<TB_UserPhoto> photos);

        /// <summary>
        /// 删除生活照
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="pictureId"></param>
        /// <returns></returns>
        DeleteLifeShowTips DeleteLifeShowImage(long userId, string token, long pictureId);

        /// <summary>
        /// 获取用户生活照
        /// </summary>
        /// <param name="userIdd"></param>
        /// <returns></returns>
        List<TB_UserPhoto> GetLifeShowImages(long userId);

        /// <summary>
        /// 视频通话记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="anchorId"></param>
        /// <param name="sender"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <param name="recordId"></param>
        /// <param name="totalFee"></param>
        /// <returns></returns>
        LiveCallTips LiveCallRecord(long userId, long anchorId, string token, long liveId, int sender, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="coinCount"></param>
        /// <param name="money"></param>
        /// <param name="cashRatio"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        CashOutTips CashOut(long userId, string token, string cashAccount, string cashName, int money, out long currentIncome);

        /// <summary>
        /// 发送礼物
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="anchorId"></param>
        /// <param name="giftId"></param>
        /// <param name="transactionId"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        SendGiftTips SendGiftTransaction(long userId, string token, long anchorId, int giftId, out long currentCoinCount);
          
        /// <summary>
        /// 设置网易账号登录状态
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        SetNeteaseLoginOrChatStatusTips SetNeteaseLoginStatus(long userId, string token, int status);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        SetNeteaseLoginOrChatStatusTips SetNeteaseChatStatus(long userId, string token, int status);

        /// <summary>
        /// 视频通话记录
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        List<LiveCallHistoryItemReturn> GetLiveCallHistory(long userId, string token, int pageIndex, int pageSize, out LiveCallHistoryTips tips);

        /// <summary>
        /// 用户发送礼物给主播时推送消息通知主播
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="anchorId"></param>
        /// <param name="giftId"></param>
        /// <returns></returns>
        void JPushMessageAfterSendGift(long userId, long anchorId, int giftId);

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="nickName"></param>
        /// <returns></returns>
        ModifyAccountInfoTips ModifyNickName(long userId, string token, string nickName);

        /// <summary>
        /// 修改用户签名
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        ModifyAccountInfoTips ModifyDescription(long userId, string token, string description);

        /// <summary>
        /// 修改用户头像
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="avatar"></param>
        /// <returns></returns>
        ModifyAccountInfoTips ModifyAvatar(long userId, string avatar, string thumbAvatarUrl);

        InviteStatisticsItem GetInviteStatistics(long userId, string token, out BasicTips tips);

        List<InviteRewardItem> GetInviteRewardList(long userId, string token, int pageIndex, int pageSize, out BasicTips tips);

        IncomeStatisticsItem GetIncomeStatistics(long userId, string token, out BasicTips tips);

        int GetUnReadMessageCount(long userId, string token);
        BasicTips SetMessageReadStatus(long userId, string token);
        BasicTips AddCoin(long userId, string token, int coinCount);
        long LiveCallRecordPerMinute(long userId, string token, long anchorId, long channelId, int seconds, out NeteaseCallNotifyTips tips);
        void SaveVisitLog(TB_AppVisitLog visitLog);
        void RandomSendMessage(long toUserId);
        void SaveWaitingSendMessage(long toUserId);
        List<NormalUserItem> GetUserList(UserCategoryConfig category, int pageIndex, int pageSize);
        SendMessageTips GetTotalRemainderTicketCount(long userId, string token, out string totalRemainderTicket);
        SendMessageTips GetChatTicketCount(long userId, string token, string accId, out int remaindCount, out bool isLimit);
        SendMessageTips RecordChatTicketCount(long userId, string token, long anchorId, int count, out bool isLimit, out int remaindTicket);
        int GetRemindMessageCount(long userId, string token,out bool isLimit);
    }
}
