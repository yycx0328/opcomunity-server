using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using System.Data.Linq.SqlClient;
using System.Data.Entity;
using Jiguang.JPush;
using Opcomunity.Services.Helpers;
using Jiguang.JPush.Model;
using log4net;
using System.Reflection;
using Utility.Common;
using System.Configuration;
using System.Collections.Specialized;
using qqzeng_ip_dat;

namespace Opcomunity.Services.Implementations
{
    public class AccountService : ServiceBase, IAccountService
    {
        protected static ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);
        //private static readonly int defaultCashRatio = TypeHelper.TryParse(ConfigurationManager.AppSettings["DefaultCashRatio"],50);

        private static int SqucenceID = 123456;

        /// <summary>
        /// 生成唯一订单号
        /// </summary>
        /// <param name="suffix"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetUniqueTransactionId(string suffix, long userId)
        {
            string strUserId = (userId % 88888888).ToString("00000000#");
            string timeSpan = (TimeHelper.ConvertToUnixDateTimeStamp(DateTime.Now)).ToString();
            SqucenceID++;
            string squcenceID = (SqucenceID % 999999).ToString("000000#");
            return string.Join("", suffix, strUserId, timeSpan, squcenceID);
        }

        public bool IsLoginUser(long userId, string token)
        { 
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                var user = query.SingleOrDefault();
                return user != null;
            }
        }

        public LoginUserDetailInfoItem GetUserDetailInfo(long userId, string token, out CheckTips tips)
        {
            using (var context = base.NewContext())
            {
                var qToken = context.TB_UserTokenInfo.FirstOrDefault(t => t.UserId == userId && t.UserToken == token);
                if (qToken == null)
                {
                    tips = CheckTips.UserNotLoginErr;
                    return null;
                }

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
                            join v in context.TB_UserVIP.Where(p=>p.StartTime<=DateTime.Now && p.EndTime > DateTime.Now)
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
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseToken = n == null ? "" : n.NeteaseToken,
                                NeteaseLoginStatus = n == null ? 0 : n.LoginStatus,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
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
                var info = query.SingleOrDefault();
                if(info!=null)
                {
                    var qVipUser = from v in context.TB_UserVIP
                                   where v.UserId == userId
                                   select v;
                    var vipUser = qVipUser.FirstOrDefault(p=>p.StartTime<=DateTime.Now && DateTime.Now < p.EndTime);
                    if(vipUser == null)
                    {
                        info.IsVip = false;
                        info.VipExpireTime = 0;
                    }
                    else
                    {
                        info.IsVip = true;
                        DateTime maxEndTime = qVipUser.Max(p => p.EndTime);
                        info.VipExpireTime = (long)(maxEndTime - unixTime).TotalSeconds;
                    }
                    tips = CheckTips.Success;
                    return info;
                }
                tips = CheckTips.Failed;
                return null; 
            }
        } 

        public bool SaveLifeShowImages(List<TB_UserPhoto> photos)
        {
            using (var context = base.NewContext())
            {
                context.TB_UserPhoto.AddRange(photos);
                return context.SaveChanges()>0;
            }
        }

        public DeleteLifeShowTips DeleteLifeShowImage(long userId, string token, long pictureId)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.FirstOrDefault() == null)
                    return DeleteLifeShowTips.UserNotExistErr;

                var qPhoto = from p in context.TB_UserPhoto where p.Id == pictureId select p;
                var userPhoto = qPhoto.SingleOrDefault();
                if (userPhoto == null)
                    return DeleteLifeShowTips.PictureNotExistErr;

                if (userPhoto.UserId != userId)
                    return DeleteLifeShowTips.CannotDeleteOthersPictureErr;

                context.TB_UserPhoto.Remove(userPhoto);
                context.SaveChanges();
                return DeleteLifeShowTips.Success;
            }
        }

        public List<TB_UserPhoto> GetLifeShowImages(long userId)
        {
            using (var context = base.NewContext())
            {
                return context.TB_UserPhoto.Where(p => p.UserId == userId).OrderByDescending(p=>p.CreateTime).ToList();
            }
        }

        public LiveCallTips LiveCallRecord(long userId, long anchorId, string token, long liveId, int sender, DateTime startTime, DateTime endTime)
        {
            using (var context = base.NewContext())
            { 
                bool isUserSend = false;
                bool isAnchorSend = false;
                if (sender == (int)LiveCallSenderConfig.User)
                    isUserSend = true;
                else if (sender == (int)LiveCallSenderConfig.Anchor)
                    isAnchorSend = true;
                else
                    return LiveCallTips.SenderErr;

                int callRatio = 0;
                if (isUserSend)
                {
                    var qUser = from userInfo in context.TB_User
                                join tokenInfo in context.TB_UserTokenInfo
                                on userInfo.Id equals tokenInfo.UserId
                                where userInfo.Id == userId && tokenInfo.UserToken == token
                                select userInfo;
                    if (qUser.SingleOrDefault() == null)
                        return LiveCallTips.UserNotExistErr;

                    var qAnchor = from u in context.TB_User
                                  join a in context.TB_Anchor
                                  on u.Id equals a.UserId
                                  where u.Id == anchorId && a.IsAuth && !a.IsBlack
                                  select a;
                    var anchor = qAnchor.SingleOrDefault();
                    if (anchor == null)
                        return LiveCallTips.AnchorUserNotExistErr;
                    callRatio = anchor.CallRatio;
                }
                else if(isAnchorSend)
                {
                    var qUser = from userInfo in context.TB_User
                                where userInfo.Id == userId 
                                select userInfo;
                    if (qUser.SingleOrDefault() == null)
                        return LiveCallTips.UserNotExistErr;

                    var qAnchor = from u in context.TB_User
                                  join t in context.TB_UserTokenInfo
                                  on u.Id equals t.UserId
                                  join a in context.TB_Anchor
                                  on u.Id equals a.UserId
                                  where u.Id == anchorId && t.UserToken == token && a.IsAuth && !a.IsBlack
                                  select a;
                    var anchor = qAnchor.SingleOrDefault();
                    if (anchor == null)
                        return LiveCallTips.AnchorUserNotExistErr;
                    callRatio = anchor.CallRatio;
                }

                int totalSecond = (int)(endTime.Subtract(startTime)).TotalSeconds;
                if (totalSecond != 0)
                    return LiveCallTips.OnlyRecordZeroTime;

                //var qCallAnchor = from ca in context.TB_CallAnchor
                //                  where ca.StartTime <= endTime && ca.EndTime >= startTime && ca.Status == 0
                //                  select ca;

                var qCallAnchor = from ca in context.TB_CallAnchor
                                  where ca.CallId == liveId && ca.Status == 0
                                  select ca;
                if (qCallAnchor.FirstOrDefault() != null)
                    return LiveCallTips.LiveRecordAlreadyExist;

                var unixTime = new DateTime(1970, 1, 1, 8, 0, 0);
                var qLiveCall = from t in ((from ca in context.TB_NeteaseCall
                                            where ca.AnchorId == anchorId && ca.Status == 0
                                            select new LiveConnectItem
                                            {
                                                IsUnConnected = false,
                                                CallTime = ca.CallTime / 1000,
                                            })
                   .Concat(from c in context.TB_CallAnchor
                           where c.AnchorId == anchorId && c.Status == 0
                           select new LiveConnectItem
                           {
                               IsUnConnected = true,
                               CallTime = DbFunctions.DiffSeconds(unixTime, c.CreateTime) ?? 0
                           }))
                                orderby t.CallTime descending
                                select t;
                var liveCall = qLiveCall.Take(2).ToList();
                if(liveCall!=null && liveCall.Where(p => !p.IsUnConnected).Count() == 0)
                {
                    var qNetease = from n in context.TB_NeteaseAccount
                                   where n.UserId == anchorId
                                   select n;
                    var netease = qNetease.SingleOrDefault();
                    if (netease != null)
                        netease.ChatStatus = (int)NeteaseChatStatusConfig.OffLine;
                }

                TB_CallAnchor model = new TB_CallAnchor()
                {
                    UserId = userId,
                    AnchorId = anchorId,
                    CallId = liveId,
                    CallRatio = callRatio,
                    Sender = sender,
                    StartTime = startTime,
                    EndTime = endTime,
                    TotalFee = 0,
                    Ip = WebUtils.GetClientIP(),
                    Status = 0,
                    StatusDescription = "成功",
                    CreateTime = DateTime.Now
                };
                context.TB_CallAnchor.Add(model);
                
                context.SaveChanges();
                return LiveCallTips.Success;
            }
        } 

        public CashOutTips CashOut(long userId, string token, string cashAccount, string cashName, int money, out long currentIncome)
        {
            using (var context = base.NewContext())
            {
                currentIncome = 0;
                #region 验证用户
                // 验证是否为登录用户
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.SingleOrDefault() == null)
                    return CashOutTips.UserAccountErr;
                // 验证用户是否为主播
                var qAnchor = from anchorInfo in context.TB_Anchor
                              where anchorInfo.UserId == userId && anchorInfo.IsAuth && !anchorInfo.IsBlack
                              select anchorInfo;

                var cashRatio = 0;
                var anchor = qAnchor.SingleOrDefault();
                if (anchor == null)
                    cashRatio = ConfigHelper.GetValue("DefaultCashRatio", 50);
                else
                    cashRatio = anchor.CashRatio;
                #endregion

                // 将人民币金额转换成系统收益金额，CashRatio是一个整数值如65表示65%，在处理的时候要注意除以100
                Log4NetHelper.Info(log, "cashRatio:"+ cashRatio);
                int coinCount = (int)Math.Ceiling((money * 10.0) * 100 / cashRatio);
                Log4NetHelper.Info(log, "coinCount:" + coinCount);

                // 获取用户收入额
                var qUserCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId && p.CurrentIncome >= coinCount);
                if (qUserCoin == null)
                    return CashOutTips.UserCoinNotEnoughErr;

                string id = GetUniqueTransactionId("CO", userId);
                #region 生成提现交易订单
                var model = new TB_CashTransaction()
                {
                    Id = id,
                    UserId = userId,
                    CashMoney = money,
                    CoinCount = coinCount,
                    CashRatio = cashRatio,
                    CashAccount = cashAccount,
                    CashName = cashName,
                    Status = (int)CashStatusConfig.Cahing,
                    StatusDescription = CashStatusConfig.Cahing.GetRemark(),
                    CashTime = DateTime.Now,
                    Ip = WebUtils.GetClientIP(),
                    UpdateDate = DateTime.Now
                };
                context.TB_CashTransaction.Add(model);
                #endregion

                #region 扣除用户收入额并记录收益支出流水
                qUserCoin.CurrentIncome -= coinCount;
                currentIncome = qUserCoin.CurrentIncome;
                var userCoinJournal = new TB_UserIncomeJournal()
                {
                    UserId = userId,
                    IncomeCount = coinCount,
                    CurrentCount = qUserCoin.CurrentIncome,
                    IOStatus = CoinIOStatusConfig.O.ToString(),
                    JournalType = (int)CoinJournalConfig.CashOut,
                    JournalDesc = CoinJournalConfig.CashOut.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserIncomeJournal.Add(userCoinJournal);
                #endregion

                #region 被邀请主播提现时，邀请人会获得10%的的收益作为奖励
                var inviteModel = context.TB_UserInvite.FirstOrDefault(p => p.NewUserId == userId);
                if (inviteModel != null)
                {
                    var inviteUserCoinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == inviteModel.UserId);
                    var inviteUserIncome = (int)(coinCount * inviteModel.CashoutAwardRate * 0.01);
                    if (inviteUserCoinModel == null)
                    {
                        inviteUserCoinModel = new TB_UserCoin()
                        {
                            UserId = inviteModel.UserId,
                            CurrentCoin = 0,
                            CurrentIncome = inviteUserIncome
                        };
                        context.TB_UserCoin.Add(inviteUserCoinModel);
                    }
                    else
                        inviteUserCoinModel.CurrentIncome += inviteUserIncome;
                    var inviteUserIncomeJournal = new TB_UserIncomeJournal()
                    {
                        UserId = inviteModel.UserId,
                        OriginUserId = userId,
                        IncomeCount = inviteUserIncome,
                        CurrentCount = inviteUserCoinModel.CurrentIncome,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.InviteReward,
                        JournalDesc = CoinJournalConfig.InviteReward.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserIncomeJournal.Add(inviteUserIncomeJournal);
                }
                #endregion

                context.SaveChanges();
                return CashOutTips.Success;
            }
        }

        public SendGiftTips SendGiftTransaction(long userId,string token, long anchorId,int giftId, out long currentCoinCount)
        {
            using (var context = base.NewContext())
            {
                currentCoinCount = 0;

                #region 验证主播及礼品是否存在
                var anchor = context.TB_Anchor.SingleOrDefault(p=> p.IsAuth && !p.IsBlack &&p.UserId == anchorId);
                if (anchor == null)
                    return SendGiftTips.AnchorUserNotExistErr;

                var gift = context.TB_Gift.SingleOrDefault(g => g.Id == giftId && g.IsAvailable);
                if (gift == null)
                    return SendGiftTips.GiftNotExistErr;
                #endregion

                #region 验证用户账户余额是否足够
                int price = gift.OriginalPrice;
                int discount = gift.Discount ?? 0;
                if (gift.IsDiscount && gift.DiscountStart <= DateTime.Now
                && gift.DiscountEnd >= DateTime.Now && discount > 0)
                {
                    var dprice = (gift.OriginalPrice * discount) / 100.0;
                    price = (int)Math.Round(dprice);
                }
                var userCoin = context.TB_UserCoin.SingleOrDefault(c => c.UserId == userId && c.CurrentCoin >= price);
                if (userCoin == null)
                    return SendGiftTips.UserCoinNotEnoughErr; 
                #endregion

                #region 生成交易订单
                var model = new TB_GiftTransaction()
                {
                    UserId = userId,
                    AnchorId = anchorId,
                    GiftId = giftId,
                    OriginalPrice = gift.OriginalPrice,
                    CostPrice = price,
                    Ip = WebUtils.GetClientIP(),
                    Status = (int)ExecuteStatusConfig.Success,
                    StatusDescription = ExecuteStatusConfig.Success.GetRemark(),
                    CreateTime = DateTime.Now
                };
                context.TB_GiftTransaction.Add(model);
                #endregion

                #region 扣除用户棉花糖并新增用户棉花糖交易流水
                currentCoinCount = userCoin.CurrentCoin - price;
                userCoin.CurrentCoin = currentCoinCount;
                var userCoinJournal = new TB_UserCoinJournal()
                {
                    UserId = userId,
                    CoinCount = price,
                    CurrentCount = currentCoinCount,
                    IOStatus = CoinIOStatusConfig.O.ToString(),
                    JournalType = (int)CoinJournalConfig.SendGift,
                    JournalDesc = CoinJournalConfig.SendGift.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserCoinJournal.Add(userCoinJournal); 
                #endregion

                #region 增加主播收入额，如果之前主播没有收入，则新增收入额，否则增加
                var anchorCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == anchorId);
                if (anchorCoin == null)
                {
                    anchorCoin = new TB_UserCoin()
                    {
                        UserId = anchorId,
                        CurrentCoin = 0,
                        CurrentIncome = price
                    };
                    context.TB_UserCoin.Add(anchorCoin);
                }
                else
                    anchorCoin.CurrentIncome += price;
                // 新增主播收益流水
                var anchorIncomeJournal = new TB_UserIncomeJournal()
                {
                    UserId = anchorId,
                    OriginUserId = userId,
                    IncomeCount = price,
                    CurrentCount = anchorCoin == null ? price : anchorCoin.CurrentIncome,
                    IOStatus = CoinIOStatusConfig.I.ToString(),
                    JournalType = (int)CoinJournalConfig.ReciveGift,
                    JournalDesc = CoinJournalConfig.ReciveGift.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserIncomeJournal.Add(anchorIncomeJournal);
                #endregion

                // 增加主播魅力值
                anchor.Glamour += price;
                
                #region 增加邀请人收益
                var inviteModel = context.TB_UserInvite.FirstOrDefault(p => p.NewUserId == userId);
                if (inviteModel != null)
                {
                    var inviteUserCoinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == inviteModel.UserId);
                    var inviteUserIncome = (int)(price * inviteModel.CostAwardRate * 0.01);
                    if (inviteUserCoinModel == null)
                    {
                        inviteUserCoinModel = new TB_UserCoin()
                        {
                            UserId = anchorId,
                            CurrentCoin = 0,
                            CurrentIncome = inviteUserIncome
                        };
                        context.TB_UserCoin.Add(inviteUserCoinModel);
                    }
                    else
                        inviteUserCoinModel.CurrentIncome += inviteUserIncome;
                    var inviteUserIncomeJournal = new TB_UserIncomeJournal()
                    {
                        UserId = inviteModel.UserId,
                        OriginUserId = userId,
                        IncomeCount = inviteUserIncome,
                        CurrentCount = inviteUserCoinModel.CurrentIncome,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.InviteReward,
                        JournalDesc = CoinJournalConfig.InviteReward.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserIncomeJournal.Add(inviteUserIncomeJournal);
                }
                #endregion

                // 提交交易
                context.SaveChanges(); 
                return SendGiftTips.Success;
            }
        }
         
        public SetNeteaseLoginOrChatStatusTips SetNeteaseLoginStatus(long userId, string token, int status)
        {
            if (!Enum.IsDefined(typeof(NeteaseLoginStatusConfig), status))
                return SetNeteaseLoginOrChatStatusTips.StatusParameterErr;

            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo; 
                if (query.SingleOrDefault() == null)
                    return SetNeteaseLoginOrChatStatusTips.UserNotLoginErr;

                var qNeteaseAccount = from n in context.TB_NeteaseAccount where n.UserId == userId select n;
                var neteaseAccount = qNeteaseAccount.SingleOrDefault();
                if (neteaseAccount == null)
                    return SetNeteaseLoginOrChatStatusTips.NeteaseAccountNotExistErr;

                neteaseAccount.LoginStatus = status;
                if (status == (int)NeteaseLoginStatusConfig.OffLine)
                    neteaseAccount.ChatStatus = (int)NeteaseChatStatusConfig.OffLine;
                else if (status == (int)NeteaseLoginStatusConfig.OnLine)
                    neteaseAccount.ChatStatus = (int)NeteaseChatStatusConfig.Free;
                neteaseAccount.UpdateTime = DateTime.Now;
                context.SaveChanges();
                return SetNeteaseLoginOrChatStatusTips.Success;
            }
        }

        public SetNeteaseLoginOrChatStatusTips SetNeteaseChatStatus(long userId, string token, int status)
        {
            if (!Enum.IsDefined(typeof(NeteaseChatStatusConfig), status))
                return SetNeteaseLoginOrChatStatusTips.StatusParameterErr;

            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return SetNeteaseLoginOrChatStatusTips.UserNotLoginErr;

                var qNeteaseAccount = from n in context.TB_NeteaseAccount where n.UserId == userId select n;
                var neteaseAccount = qNeteaseAccount.SingleOrDefault();
                if (neteaseAccount == null)
                    return SetNeteaseLoginOrChatStatusTips.NeteaseAccountNotExistErr;

                neteaseAccount.ChatStatus = status;
                neteaseAccount.UpdateTime = DateTime.Now;
                context.SaveChanges();
                return SetNeteaseLoginOrChatStatusTips.Success;
            }
        }

        public List<LiveCallHistoryItemReturn> GetLiveCallHistory(long userId, string token, int pageIndex, int pageSize, out LiveCallHistoryTips tips)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.SingleOrDefault() == null)
                {
                    tips = LiveCallHistoryTips.UserNotLoginErr;
                    return null;
                }
                var unixTime = new DateTime(1970,1,1,8,0,0);
                var query = from t in ((from ca in context.TB_NeteaseCall
                             join u in context.TB_User
                             on ca.AnchorId equals u.Id
                             where ca.CallerId == userId && ca.Status == 0
                             select new LiveCallHistoryItem
                             {
                                 IsUnConnected = false,
                                 IsAnchor = true,
                                 UserId = u.Id,
                                 UserName = u.NickName,
                                 UserAvatar = u.Avatar,
                                 ThumbnailAvatar = u.ThumbnailAvatar,
                                 CallRatio = ca.CallRatio,
                                 TotalFee = ca.ActualTransferFee,
                                 CallTime = ca.CallTime / 1000,
                                 TotalSecond = ca.Duration
                             })
                    .Concat(from ca in context.TB_NeteaseCall
                           join u in context.TB_User
                           on ca.CallerId equals u.Id
                           where ca.AnchorId == userId && ca.Status == 0
                           select new LiveCallHistoryItem
                           {
                               IsUnConnected = false,
                               IsAnchor = false,
                               UserId = u.Id,
                               UserName = u.NickName,
                               UserAvatar = u.Avatar,
                               ThumbnailAvatar = u.ThumbnailAvatar,
                               CallRatio = ca.CallRatio,
                               TotalFee = ca.TotalFee,
                               CallTime = ca.CallTime / 1000,
                               TotalSecond = ca.Duration
                           })
                   .Concat(from c in context.TB_CallAnchor
                          join u in context.TB_User
                          on c.UserId equals u.Id
                          where c.AnchorId == userId && c.Status == 0
                          select new LiveCallHistoryItem
                          {
                              IsUnConnected = true,
                              IsAnchor = false,
                              UserId = u.Id,
                              UserName = u.NickName,
                              UserAvatar = u.Avatar,
                              ThumbnailAvatar = u.ThumbnailAvatar,
                              CallRatio = c.CallRatio,
                              TotalFee = c.TotalFee,
                              CallTime = DbFunctions.DiffSeconds(unixTime, c.CreateTime) ?? 0,
                              TotalSecond = 0
                          })
                   .Concat(from c in context.TB_CallAnchor
                          join u in context.TB_User
                          on c.AnchorId equals u.Id
                          where c.UserId == userId && c.Status == 0
                          select new LiveCallHistoryItem
                          {
                              IsUnConnected = true,
                              IsAnchor = true,
                              UserId = u.Id,
                              UserName = u.NickName,
                              UserAvatar = u.Avatar,
                              ThumbnailAvatar = u.ThumbnailAvatar,
                              CallRatio = c.CallRatio,
                              TotalFee = c.TotalFee,
                              CallTime = DbFunctions.DiffSeconds(unixTime, c.CreateTime) ?? 0,
                              TotalSecond = 0
                          }))
                    join v in context.TB_UserVIP.Where(p => p.StartTime<=DateTime.Now && p.EndTime>DateTime.Now)
                    on t.UserId equals v.UserId
                    into g
                    from v in g.DefaultIfEmpty()
                    select new LiveCallHistoryItemReturn
                    {
                        IsUnConnected = t.IsUnConnected,
                        IsAnchor = t.IsAnchor,
                        UserId = t.UserId,
                        UserName = t.UserName,
                        UserAvatar = t.UserAvatar,
                        ThumbnailAvatar = t.ThumbnailAvatar,
                        IsVip = v!=null,
                        CallRatio = t.CallRatio,
                        TotalFee = t.TotalFee,
                        CallTime = t.CallTime,
                        TotalSecond = t.TotalSecond
                    };
                
                tips = LiveCallHistoryTips.Success;
                return query.OrderByDescending(p=>p.CallTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public void JPushMessageAfterSendGift(long userId, long anchorId, int giftId)
        {
            Try(() => {
                using (var context = base.NewContext())
                {
                    var qUser = from u in context.TB_User where u.Id == userId select u;
                    var userInfo = qUser.SingleOrDefault();
                    if (userInfo == null)
                        return;

                    var qGift = from g in context.TB_Gift where g.Id == giftId select g;
                    var giftInfo = qGift.SingleOrDefault();
                    if (giftInfo == null)
                        return;

                    string msgTitle = "收到礼物";
                    string msgContent = string.Format("{0}给您发送了礼物{1}，赶紧查看吧！", userInfo.NickName, giftInfo.Name);
                    Dictionary<string, string> msgExtras = new Dictionary<string, string>();
                    msgExtras.Add("UserId", userId.ToString());
                    msgExtras.Add("UserName", userInfo.NickName);
                    msgExtras.Add("UserAvatar", userInfo.ThumbnailAvatar);
                    msgExtras.Add("GiftId", giftId.ToString());
                    msgExtras.Add("GiftName", giftInfo.Name);
                    msgExtras.Add("GiftConver", giftInfo.Conver);
                    Message msg = new Message
                    {
                        Title = msgTitle,
                        ContentType = "text",
                        Content = msgContent,
                        Extras = msgExtras
                    };

                    List<string> listAlias = new List<string>();
                    listAlias.Add(anchorId.ToString());
                    Audience audience = new Audience
                    {
                        Alias = listAlias
                    };

                    JPushClient client = new JPushClient(ConfigHelper.GetValue("JPushAppKey"), ConfigHelper.GetValue("JPushMasterSecret"));
                    PushPayload pushPayload = new PushPayload
                    {
                        Platform = "all",
                        Message = msg,
                        Audience = audience
                    };
                    var response = client.SendPush(pushPayload);
                    Log4NetHelper.Info(log, response.Content);
                }
            });
        }

        public ModifyAccountInfoTips ModifyAvatar(long userId, string avatar, string thumbAvatarUrl)
        {
            using (var context = base.NewContext())
            {
                var user = context.TB_User.SingleOrDefault(p => p.Id == userId);
                if (user == null)
                    return ModifyAccountInfoTips.UserNotExistErr;
                user.Avatar = avatar;
                user.ThumbnailAvatar = thumbAvatarUrl;

                if (context.SaveChanges() > 0)
                {
                    var neteaseAccount = context.TB_NeteaseAccount.SingleOrDefault(p => p.UserId == userId);
                    if (neteaseAccount != null)
                    {
                        NameValueCollection data = new NameValueCollection();
                        data.Add("accid", neteaseAccount.NeteaseAccId);
                        data.Add("icon", avatar);
                        string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.CRT_UPDATEUSER_URL, data);
                        Log4NetHelper.Info(log, "更新网易云账号头像：" + result);
                    }
                    return ModifyAccountInfoTips.Success;
                }
                return ModifyAccountInfoTips.Failed;
            }
        }

        public ModifyAccountInfoTips ModifyDescription(long userId,string token, string description)
        {
            using (var context = base.NewContext())
            { 
                var qToken = from t in context.TB_UserTokenInfo
                             where t.UserId == userId && t.UserToken == token
                             select t;
                if (qToken.FirstOrDefault() == null)
                    return ModifyAccountInfoTips.UserNotLoginErr;

                var qUser = from u in context.TB_User where u.Id == userId select u;
                var user = qUser.SingleOrDefault();
                if (user == null)
                    return ModifyAccountInfoTips.UserNotExistErr;
                user.Description = description;
                if (context.SaveChanges() > 0)
                    return ModifyAccountInfoTips.Success;
                return ModifyAccountInfoTips.Failed;
            }
        }

        public ModifyAccountInfoTips ModifyNickName(long userId, string token, string nickName)
        {
            using (var context = base.NewContext())
            {
                var qToken = from t in context.TB_UserTokenInfo
                             where t.UserId == userId && t.UserToken == token select t;
                if(qToken.FirstOrDefault()==null)
                    return ModifyAccountInfoTips.UserNotLoginErr;

                var qUser = from u in context.TB_User where u.Id == userId select u;
                var user = qUser.SingleOrDefault();
                if (user == null)
                    return ModifyAccountInfoTips.UserNotExistErr;
                if (nickName == user.NickName)
                    return ModifyAccountInfoTips.NickNameSameAsOldName;
                user.NickName = nickName;
                if (context.SaveChanges() > 0)
                {
                    var neteaseAccount = context.TB_NeteaseAccount.SingleOrDefault(p => p.UserId == userId);
                    if (neteaseAccount != null)
                    { 
                        NameValueCollection data = new NameValueCollection();
                        data.Add("accid", neteaseAccount.NeteaseAccId);
                        data.Add("name", nickName);
                        string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.CRT_UPDATEUSER_URL, data);
                        Log4NetHelper.Info(log, "更新网易云账号昵称：" + result);
                    }

                    return ModifyAccountInfoTips.Success;
                }
                return ModifyAccountInfoTips.Failed;
            }
        }

        public InviteStatisticsItem GetInviteStatistics(long userId, string token, out BasicTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                {
                    tips = BasicTips.UserNotLoginErr;
                    return null;
                }

                var qTotalReward = from income in context.TB_UserIncomeJournal.Where(p => p.UserId == userId
                                   && p.JournalType == (int)CoinJournalConfig.InviteReward)
                                   select income.IncomeCount;
                var listTotalReward = qTotalReward.ToList();
                long totalReward = 0;
                if (listTotalReward != null && listTotalReward.Count > 0)
                    totalReward = qTotalReward.Sum();

                var qTotalUser = from i in context.TB_UserInvite.Where(p => p.UserId == userId && p.NewUserId != null) select i.Id;
                int inviteUserCount = qTotalUser.Count();

                tips = BasicTips.Success;
                return new InviteStatisticsItem { TotalReward = totalReward, TotalInviteUser = inviteUserCount };
            }
        }

        public List<InviteRewardItem> GetInviteRewardList(long userId, string token,int pageIndex, int pageSize, out BasicTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                {
                    tips = BasicTips.UserNotLoginErr;
                    return null;
                }

                var qInviteReward = from i in context.TB_UserInvite.Where(p=>p.UserId==userId && p.NewUserId !=null)
                                    join u in context.TB_User
                                    on i.NewUserId equals u.Id
                                    join t in (from ic in context.TB_UserIncomeJournal.Where(p => p.UserId == userId 
                                               && p.JournalType == (int)CoinJournalConfig.InviteReward)
                                               group ic by ic.OriginUserId into g select new { OriginUserId = g.Key, TotalReward= g.Sum(p => p.IncomeCount) })
                                    on i.NewUserId equals t.OriginUserId
                                    into tmp 
                                    from t in tmp.DefaultIfEmpty()
                                    select new InviteRewardItem
                                    {
                                        UserId = i.NewUserId,
                                        NickName = u.NickName,
                                        Avatar = u.Avatar,
                                        ThumbnailAvatar = u.ThumbnailAvatar,
                                        TotalReward = t == null?0: t.TotalReward
                                    };
                 
                //var qInviteReward = from income in context.TB_UserIncomeJournal.Where(p => p.UserId == userId && p.JournalType == (int)CoinJournalConfig.InviteReward)
                //                    join i in context.TB_UserInvite.Where(p=>p.UserId == userId)
                //                    on income.OriginUserId equals i.NewUserId
                //                    group income by new { OriginUserId = income.OriginUserId }
                //                    into jg
                //                    select new { OriginUserId = jg.Select(p => p.OriginUserId).FirstOrDefault(), TotalReward = jg.Sum(p => p.IncomeCount) }
                //                    into t
                //                    join u in context.TB_User on t.OriginUserId equals u.Id
                //                    select new InviteRewardItem
                //                    {
                //                        UserId = t.OriginUserId,
                //                        NickName = u.NickName,
                //                        Avatar = u.Avatar,
                //                        TotalReward = t.TotalReward
                //                    };
                
                tips = BasicTips.Success;
                return qInviteReward.OrderByDescending(p => p.TotalReward).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public IncomeStatisticsItem GetIncomeStatistics(long userId, string token, out BasicTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                {
                    tips = BasicTips.UserNotLoginErr;
                    return null;
                }

                IncomeStatisticsItem model = new IncomeStatisticsItem();

                // 当前收益
                var curIncomeModel = context.TB_UserCoin.FirstOrDefault(p => p.UserId == userId);
                if (curIncomeModel != null) model.CurrentIncome = curIncomeModel.CurrentIncome;

                // 总收益
                var totalIncomeModel = context.TB_UserIncomeJournal.Where(p => p.UserId == userId).ToList(); 
                if (totalIncomeModel != null && totalIncomeModel.Count > 0)
                    model.TotalIncome = totalIncomeModel.Sum(p=>p.IncomeCount);
                else
                    model.TotalIncome = model.CurrentIncome;

                // 总邀请用户数
                var totalUser = context.TB_UserInvite.Where(p => p.UserId == userId && p.NewUserId != null).Count();
                model.TotalInviteUser = totalUser;

                // 总通话时长
                var liveCallModel = context.TB_NeteaseCall.Where(p => p.AnchorId == userId).ToList();
                if (liveCallModel != null && liveCallModel.Count > 0)
                    model.TotalLiveCallTime = liveCallModel.Sum(p=>p.Duration);

                // 总礼物数
                var totalGift = context.TB_GiftTransaction.Where(p => p.AnchorId == userId).Count();
                model.TotalGiftCount = totalGift;

                tips = BasicTips.Success;
                return model;
            }
        }

        public int GetUnReadMessageCount(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return -1;

                var qMessage = from m in context.TB_Message where m.UserId == userId && m.IsRead != true select m;
                return qMessage.Count();
            }
        }

        public BasicTips SetMessageReadStatus(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return BasicTips.UserNotLoginErr;

                var qMessage = from m in context.TB_Message where m.UserId == userId select m;
                foreach (var item in qMessage)
                    item.IsRead = true;
                context.SaveChanges();
                return BasicTips.Success;
            }
        }

        public BasicTips AddCoin(long userId,string token, int coinCount)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return BasicTips.UserNotLoginErr;

                var coinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId);
                if (coinModel == null)
                {
                    coinModel = new TB_UserCoin()
                    {
                        UserId = userId,
                        CurrentCoin = coinCount,
                        CurrentIncome = 0
                    };
                    context.TB_UserCoin.Add(coinModel);
                }
                else
                    coinModel.CurrentCoin += coinCount;
                var userCoinJournal = new TB_UserCoinJournal()
                {
                    UserId = userId,
                    CoinCount = coinCount,
                    CurrentCount = coinModel.CurrentCoin,
                    IOStatus = CoinIOStatusConfig.I.ToString(),
                    JournalType = (int)CoinJournalConfig.HandIn,
                    JournalDesc = CoinJournalConfig.HandIn.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserCoinJournal.Add(userCoinJournal);
                context.SaveChanges();
                return BasicTips.Success;
            }
        }

        public UserDetailInfoItem GetOthersDetailInfo(long userId, out CheckTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from u in context.TB_User
                            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                            on u.Id equals a.UserId
                            into ua
                            from a in ua.DefaultIfEmpty()
                            join n in context.TB_NeteaseAccount
                            on u.Id equals n.UserId
                            into un
                            from n in un.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on u.Id equals v.UserId
                            into uv
                            from v in uv.DefaultIfEmpty()
                            where u.Id == userId
                            select new UserDetailInfoItem
                            {
                                UserId = u.Id,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Birthday = u.Birthday,
                                Height = u.Height,
                                Weight = u.Weight,
                                Constellation = u.Constellation,
                                IsAnchor = a != null,
                                Glamour = a == null ? 0 : a.Glamour,
                                AuthStatus = a == null ? (int)AnchorAuthStatusConfig.None : (int)AnchorAuthStatusConfig.Auth,
                                CallRatio = a == null ? 0 : a.CallRatio,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseLoginStatus = n == null ? 0 : n.LoginStatus,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                IsVip = v != null,
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
                tips = CheckTips.Success;
                return query.SingleOrDefault();
            }
        }

        public TB_UserCoin GetCoinCount(long userId, string token, out BasicTips tips)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                {
                    tips = BasicTips.UserNotLoginErr;
                    return null;
                }

                var userCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId);
                tips = BasicTips.Success;
                return userCoin;
            }
        }

        public UserDetailInfoItem GetUserInfoByAccId(string accId)
        {
            using (var context = base.NewContext())
            { 
                var query = from n in context.TB_NeteaseAccount.Where(p=>p.NeteaseAccId == accId)
                            join u in context.TB_User
                            on n.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                            on n.UserId equals a.UserId
                            into ua
                            from a in ua.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on u.Id equals v.UserId
                            into uv
                            from v in uv.DefaultIfEmpty()
                            select new UserDetailInfoItem
                            {
                                UserId = u.Id,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Birthday = u.Birthday,
                                Height = u.Height,
                                Weight = u.Weight,
                                Constellation = u.Constellation,
                                IsAnchor = a !=null,
                                Glamour = a == null ? 0 : a.Glamour,
                                AuthStatus =  a== null ? (int)AnchorAuthStatusConfig.None:(int)AnchorAuthStatusConfig.Auth,
                                CallRatio = a == null ? 0:a.CallRatio,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseLoginStatus = n == null ? 0 : n.LoginStatus,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                IsVip = v != null,
                                UserPhotoItems = from up in context.TB_UserPhoto
                                                 where up.UserId == n.UserId
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

        public long LiveCallRecordPerMinute(long userId,string token, long anchorId, long channelId, int seconds, out NeteaseCallNotifyTips tips)
        {

            using (var context = base.NewContext())
            {
                // 验证登录态
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                {
                    tips = NeteaseCallNotifyTips.UserNotLoginErr;
                    return 0;
                }

                // 验证主播
                var anchor = context.TB_Anchor.SingleOrDefault(a => a.UserId == anchorId);
                if (anchor == null)
                {
                    tips = NeteaseCallNotifyTips.AnchorAccountIsNotExistErr;
                    return 0;
                }

                // 验证金额
                var qCoin = from c in context.TB_UserCoin where c.UserId == userId && c.CurrentCoin > 0 select c;
                var userCoinModel = qCoin.SingleOrDefault();
                if (userCoinModel == null)
                {
                    tips = NeteaseCallNotifyTips.UserCoinNotEnoughErr;
                    return 0;
                }

                int callRatio = anchor.CallRatio;
                int totalFee = (int)Math.Ceiling(seconds / 60.0) * callRatio;
                // 当账户金额不足的时候，实际交易金额就是账户余额
                int actualTransferFee = totalFee;
                if (userCoinModel.CurrentCoin < totalFee)
                    actualTransferFee = (int)userCoinModel.CurrentCoin;
                
                #region 解析回调参数
                // 验证通话记录是否存在，避免重复记录
                var callModel = context.TB_NeteaseCall.SingleOrDefault(c => c.ChannelId == channelId);
                if (callModel != null)
                {
                    callModel.ActualTransferFee += actualTransferFee;
                    callModel.CallTime = TimeHelper.ConvertBJTimeToUnixDateTimeStampMilliseconds(DateTime.Now);
                    callModel.Duration += seconds;
                    callModel.TotalDuration += seconds * 2;
                    callModel.TotalFee += totalFee;
                }
                else
                {
                    callModel = new TB_NeteaseCall() {
                        ChannelId = channelId,
                        CallerId = userId,
                        AnchorId = anchorId,
                        CallRatio = anchor.CallRatio,
                        TotalDuration = seconds * 2,
                        Duration = seconds,
                        TotalFee = totalFee,
                        ActualTransferFee = actualTransferFee,
                        Ext = string.Format("{0}->{1}",userId,anchorId),
                        CallTime = TimeHelper.ConvertBJTimeToUnixDateTimeStampMilliseconds(DateTime.Now),
                        EventType = 0,
                        Type= "VEDIO",
                        CallStatus = "SUCCESS",
                        Status = 0,
                        StatusDescription = "成功",
                        Live = 0,
                        CreateTime = DateTime.Now
                    };
                    context.TB_NeteaseCall.Add(callModel);
                }
                #endregion

                #region 扣除用户交易金额，并记录消费流水
                var remindCoin = userCoinModel.CurrentCoin - actualTransferFee;
                // 扣除用户余额
                userCoinModel.CurrentCoin = remindCoin;
                var userCoinJournal = new TB_UserCoinJournal()
                {
                    UserId = userId,
                    CoinCount = actualTransferFee,
                    CurrentCount = remindCoin,
                    IOStatus = CoinIOStatusConfig.O.ToString(),
                    JournalType = (int)CoinJournalConfig.LiveOutcome,
                    JournalDesc = CoinJournalConfig.LiveOutcome.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserCoinJournal.Add(userCoinJournal);
                #endregion

                #region 增加主播收入额，如果之前主播没有收入，则新增收入额，否则增加
                var anchorCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == anchorId);
                if (anchorCoin == null)
                {
                    anchorCoin = new TB_UserCoin()
                    {
                        UserId = anchorId,
                        CurrentCoin = 0,
                        CurrentIncome = actualTransferFee
                    };
                    context.TB_UserCoin.Add(anchorCoin);
                }
                else
                    anchorCoin.CurrentIncome += actualTransferFee;
                var anchorIncomeJournal = new TB_UserIncomeJournal()
                {
                    UserId = anchorId,
                    OriginUserId = userId,
                    IncomeCount = actualTransferFee,
                    CurrentCount = anchorCoin == null ? actualTransferFee : anchorCoin.CurrentIncome,
                    IOStatus = CoinIOStatusConfig.I.ToString(),
                    JournalType = (int)CoinJournalConfig.LiveIncome,
                    JournalDesc = CoinJournalConfig.LiveIncome.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserIncomeJournal.Add(anchorIncomeJournal);
                #endregion

                var qAnchor = context.TB_Anchor.SingleOrDefault(a => a.UserId == anchorId);
                // 增加主播魅力值
                qAnchor.Glamour += actualTransferFee;

                #region 增加邀请人收益
                var inviteModel = context.TB_UserInvite.FirstOrDefault(p => p.NewUserId == userId);
                if (inviteModel != null)
                {
                    var inviteUserCoinModel = context.TB_UserCoin.SingleOrDefault(p => p.UserId == inviteModel.UserId);
                    var inviteUserIncome = (int)(actualTransferFee * inviteModel.CostAwardRate * 0.01);
                    if (inviteUserCoinModel == null)
                    {
                        inviteUserCoinModel = new TB_UserCoin()
                        {
                            UserId = inviteModel.UserId,
                            CurrentCoin = 0,
                            CurrentIncome = inviteUserIncome
                        };
                        context.TB_UserCoin.Add(inviteUserCoinModel);
                    }
                    else
                        inviteUserCoinModel.CurrentIncome += inviteUserIncome;
                    var inviteUserIncomeJournal = new TB_UserIncomeJournal()
                    {
                        UserId = inviteModel.UserId,
                        OriginUserId = userId,
                        IncomeCount = inviteUserIncome,
                        CurrentCount = inviteUserCoinModel.CurrentIncome,
                        IOStatus = CoinIOStatusConfig.I.ToString(),
                        JournalType = (int)CoinJournalConfig.InviteReward,
                        JournalDesc = CoinJournalConfig.InviteReward.GetRemark(),
                        Ip = WebUtils.GetClientIP(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserIncomeJournal.Add(inviteUserIncomeJournal);
                }
                #endregion
                
                context.SaveChanges();
                tips = NeteaseCallNotifyTips.Success;
                return remindCoin;
            }
        }

        public void SaveVisitLog(TB_AppVisitLog visitLog)
        {
            using (var context = base.NewContext())
            {
                var query = from n in context.TB_NeteaseAccount
                            where n.UserId == visitLog.UserId
                            select n;
                var neteaseAccount = query.SingleOrDefault();
                if(neteaseAccount!=null)
                {
                    neteaseAccount.UpdateTime = DateTime.Now;
                }
                context.TB_AppVisitLog.Add(visitLog);
                context.SaveChanges();
            }
        }

        public void RandomSendMessage(long toUserId)
        {
            try
            {
                using (var context = base.NewContext())
                {
                    var qAnchor = from a in context.TB_Anchor
                                  where a.UserId == toUserId && a.IsAuth
                                  select a;
                    if (qAnchor.SingleOrDefault() != null)
                        return;

                    var qToNeteaseAccount = from n in context.TB_NeteaseAccount
                                where n.UserId == toUserId
                                select n;
                    var toNeteaseAccount = qToNeteaseAccount.SingleOrDefault();
                    if (toNeteaseAccount == null)
                        return;
                    var qFromNeteaseAccount = from a in context.TB_Anchor
                                              join n in context.TB_NeteaseAccount
                                              on a.UserId equals n.UserId
                                              where a.IsAuth && !a.IsBlack
                                              orderby Guid.NewGuid()
                                              select n;
                    var fromNeteaseAccount = qFromNeteaseAccount.FirstOrDefault();
                    if (fromNeteaseAccount == null)
                        return;

                    var qMessage = from c in context.TB_NeteaseMessageConfig
                                   where c.IsAvaiable
                                  orderby Guid.NewGuid()
                                  select c;
                    var message = qMessage.FirstOrDefault();
                    if (message == null)
                        return;

                    Log4NetHelper.Info(log, "message:" + message.Body);
                    NameValueCollection data = new NameValueCollection();
                    data.Add("from", fromNeteaseAccount.NeteaseAccId);
                    data.Add("ope","0");
                    data.Add("to", toNeteaseAccount.NeteaseAccId);
                    data.Add("type", message.Type.ToString());
                    data.Add("body", message.Body);
                    string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.SEND_MSG, data);
                    Log4NetHelper.Info(log, "result:"+result);
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info(log, "Message:" + ex.Message + "    StackTrace:" + ex.StackTrace);
            }
        }
        public void SaveWaitingSendMessage(long toUserId)
        {
            try
            {
                using (var context = base.NewContext())
                {
                    #region 主播不发
                    var qAnchor = from a in context.TB_Anchor
                                  where a.UserId == toUserId && a.IsAuth && !a.IsBlack
                                  select a;
                    if (qAnchor.SingleOrDefault() != null)
                        return;
                    #endregion

                    #region 已经发过的不发
                    var qSendUser = from u in context.TB_NeteaseMessageUser
                                    where u.UserId == toUserId
                                    select u;

                    if (qSendUser.SingleOrDefault() != null)
                        return;
                    #endregion

                    #region 无网易云账号的不发
                    var qToNeteaseAccount = from n in context.TB_NeteaseAccount
                                            where n.UserId == toUserId
                                            select n;
                    var toNeteaseAccount = qToNeteaseAccount.SingleOrDefault();
                    if (toNeteaseAccount == null)
                        return;
                    #endregion

                    #region VIP用户不发
                    var qVipUser = from v in context.TB_UserVIP where v.UserId == toUserId select v;
                    if (qVipUser.FirstOrDefault() != null)
                        return;
                    #endregion

                    #region 充值过邮票的用户不发
                    var qTicketUser = from t in context.TB_UserTicket where t.UserId == toUserId select t;
                    if (qTicketUser.FirstOrDefault() != null)
                        return; 
                    #endregion

                    #region 话术配置
                    var qMsgConfig = from c in context.TB_NeteaseMsgConfig
                                     where c.IsAvaiable
                                     select c;
                    var listMsgConfig = qMsgConfig.ToList();
                    if (listMsgConfig == null)
                        return; 
                    #endregion

                    var listAnchor = listMsgConfig.GroupBy(p => p.AnchorId).Select(p=>p.Key).ToList();
                    var time = DateTime.Now;
                    var listWaitingSend = new List<TB_NeteaseMessageSend>();
                    foreach(var anchorId in listAnchor)
                    {
                        var qFromNeteaseAccount = from a in context.TB_Anchor
                                                  join n in context.TB_NeteaseAccount
                                                  on a.UserId equals n.UserId
                                                  where a.UserId == anchorId && a.IsAuth && !a.IsBlack
                                                  select n;
                        var fromNeteaseAccount = qFromNeteaseAccount.SingleOrDefault();
                        if (fromNeteaseAccount == null)
                            return;

                        var listAnchorMsg = listMsgConfig.Where(p => p.AnchorId == anchorId);
                        //string location = "";
                        foreach(var msg in listAnchorMsg)
                        {
                            //var body = msg.Body;
                            //if (msg.Body.IndexOf("{location}") >= 0)
                            //{
                            //    if(string.IsNullOrEmpty(location))
                            //        location = LocationCore.GetLocationCity();
                            //    if (string.IsNullOrEmpty(location))
                            //        body = string.Empty;
                            //    else
                            //        body = body.Replace("{location}", location);
                            //}

                            var body = msg.Body;
                            if(msg.Body.IndexOf("{CityShortName}") >= 0
                                || msg.Body.IndexOf("{CityFullName}") >= 0
                                || msg.Body.IndexOf("{CityLocation}")>=0)
                            {
                                body = string.Empty;
                                var ip = WebUtils.GetClientIP();
                                var cityInfo = IPSearch2Fast.Instance.Query(ip);
                                if(!string.IsNullOrEmpty(cityInfo))
                                {
                                    Log4NetHelper.Info(log,"CityInfo:"+ cityInfo);
                                    var arrCity = cityInfo.Split('|');
                                    if(arrCity.Length>=4)
                                    {
                                        var city = arrCity[3];
                                        if(!string.IsNullOrEmpty(city))
                                        {
                                            city = city.Replace("市","");
                                            var queryCity = from c in context.TB_CityConfig
                                                            where c.CityShortName == city
                                                            select c;
                                            var cityConfig = queryCity.FirstOrDefault();
                                            if(cityConfig!=null)
                                            {
                                                if (msg.Body.IndexOf("{CityShortName}") >= 0 && !string.IsNullOrEmpty(cityConfig.CityShortName))
                                                    body = msg.Body.Replace("{CityShortName}", cityConfig.CityShortName);
                                                else if (msg.Body.IndexOf("{CityFullName}") >= 0 && !string.IsNullOrEmpty(cityConfig.CityFullName))
                                                    body = msg.Body.Replace("{CityFullName}", cityConfig.CityFullName);
                                                else if (msg.Body.IndexOf("{CityLocation}") >= 0 && !string.IsNullOrEmpty(cityConfig.CityLocation))
                                                    body = msg.Body.Replace("{CityLocation}", cityConfig.CityLocation);
                                            }
                                        }
                                    }
                                }
                            }

                            if(!string.IsNullOrEmpty(body))
                            {
                                listWaitingSend.Add(
                                    new TB_NeteaseMessageSend()
                                    {
                                        FromUserId = anchorId,
                                        FromAccId = fromNeteaseAccount.NeteaseAccId,
                                        ToUserId = toUserId,
                                        ToAccId = toNeteaseAccount.NeteaseAccId,
                                        Type = msg.Type,
                                        Body = body,
                                        WaitingSendTime = time.AddSeconds(msg.Seconds)
                                    }
                                );
                            }
                        }
                    }
                    context.TB_NeteaseMessageSend.AddRange(listWaitingSend);
					var msgUser = new TB_NeteaseMessageUser()
					{
						UserId = toUserId,
						CreateTime = DateTime.Now
					};
					context.TB_NeteaseMessageUser.Add(msgUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log4NetHelper.Info(log, "Message:" + ex.Message + "    StackTrace:" + ex.StackTrace);
            }
        }

        public List<NormalUserItem> GetUserList(UserCategoryConfig category, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var unixTime = new DateTime(1970, 1, 1, 8, 0, 0);
                if (category == (int)UserCategoryConfig.Normal)
                {
                    var query = from n in context.TB_NeteaseAccount
                                join u in context.TB_User
                                on n.UserId equals u.Id
                                join a in context.TB_Anchor.Where(p=>p.IsAuth && !p.IsBlack)
                                on n.UserId equals a.UserId
                                into g
                                from a in g.DefaultIfEmpty()
                                join v in context.TB_UserVIP.Where(p=>p.StartTime <= DateTime.Now && p.EndTime>DateTime.Now)
                                on n.UserId equals v.UserId
                                into t
                                from v in t.DefaultIfEmpty()
                                where a == null && v == null
                                select new NormalUserItem()
                                {
                                    UserId = n.UserId,
                                    NickName = u.NickName,
                                    Avatar = u.Avatar,
                                    ThumbnailAvatar = u.ThumbnailAvatar,
                                    Description = u.Description,
                                    IsVip = v!=null,
                                    OnlineTime = DbFunctions.DiffSeconds(unixTime, n.UpdateTime ?? n.CreateTime)
                                };
                    return query.OrderByDescending(p => p.OnlineTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
                else
                {
                    var query = from n in context.TB_NeteaseAccount
                                join u in context.TB_User
                                on n.UserId equals u.Id
                                join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                                on n.UserId equals a.UserId
                                into g
                                from a in g.DefaultIfEmpty()
                                join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                                on n.UserId equals v.UserId
                                into t
                                from v in t.DefaultIfEmpty()
                                where a == null && v != null
                                select new NormalUserItem()
                                {
                                    UserId = n.UserId,
                                    NickName = u.NickName,
                                    Avatar = u.Avatar,
                                    ThumbnailAvatar = u.ThumbnailAvatar,
                                    Description = u.Description,
                                    IsVip = v != null,
                                    OnlineTime = DbFunctions.DiffSeconds(unixTime, n.UpdateTime ?? n.CreateTime)
                                };
                    return query.OrderByDescending(p => p.OnlineTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();

                    //var query = from n in context.TB_NeteaseAccount
                    //            join u in context.TB_User
                    //            on n.UserId equals u.Id
                    //            join t in (from uc in context.TB_UserCoinJournal
                    //                       where uc.IOStatus == CoinIOStatusConfig.O.ToString()
                    //                       group uc by uc.UserId into gt
                    //                       select new { UserId = gt.Key, TotalCost = gt.Sum(p => p.CoinCount)
                    //            }) 
                    //            on n.UserId equals t.UserId
                    //            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                    //            on n.UserId equals a.UserId
                    //            into g
                    //            from a in g.DefaultIfEmpty()
                    //            where a == null
                    //            orderby t.TotalCost descending
                    //            select new NormalUserItem()
                    //            {
                    //                UserId = n.UserId,
                    //                NickName = u.NickName,
                    //                Avatar = u.Avatar,
                    //                ThumbnailAvatar = u.ThumbnailAvatar,
                    //                Description = u.Description,
                    //                OnlineTime = DbFunctions.DiffSeconds(unixTime, n.UpdateTime ?? n.CreateTime)
                    //            };
                    //return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
            }
        }

        public SendMessageTips GetTotalRemainderTicketCount(long userId, string token, out string totalRemainderTicket)
        {
            using (var context = base.NewContext())
            {
                totalRemainderTicket = "0";
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return SendMessageTips.UserNotLoginErr;

                var qTickets = from t in context.TB_UserTicket where t.UserId == userId select t;
                var tickets = qTickets.ToList();
                if (tickets == null || tickets.Count == 0)
                    return SendMessageTips.UserNotTicketErr;

                if(tickets.FirstOrDefault(p=>p.Category == OrderTypeConfig.UnLimit.ToString())!=null)
                {
                    totalRemainderTicket = "无限制";
                }
                else
                {
                    totalRemainderTicket = tickets.Sum(p => p.RemainderTicket).ToString();
                }
                return SendMessageTips.Success;
            }
        }

        public SendMessageTips GetChatTicketCount(long userId, string token,string accId, out int remaindCount, out bool isLimit)
        { 
            using (var context = base.NewContext())
            {
                remaindCount = 0;
                isLimit = true;
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return SendMessageTips.UserNotLoginErr;
                
                var qAnchor = from n in context.TB_NeteaseAccount
                              join a in context.TB_Anchor
                              on n.UserId equals a.UserId
                              where a.IsAuth && !a.IsBlack && n.NeteaseAccId == accId
                              select a;
                var anchor = qAnchor.SingleOrDefault();
                if (anchor == null)
                    return SendMessageTips.AnchorNotExistErr;
                
                var qVipUser = from v in context.TB_UserVIP
                               where v.UserId == userId && v.StartTime <= DateTime.Now && v.EndTime > DateTime.Now
                               select v;
                var vipUser = qVipUser.FirstOrDefault();

                var qTickets = from t in context.TB_UserTicket where t.UserId == userId select t;
                var tickets = qTickets.ToList();

                if (tickets == null || tickets.Count == 0)
                {
                    var qNeteaseText = from t in context.TB_NeteaseText
                                       where t.FromUserId == userId
                                       select t;
                    if(qNeteaseText.FirstOrDefault()==null)
                    {
                        isLimit = true;
                        remaindCount = 1;
                        return SendMessageTips.Success;
                    }
                    return SendMessageTips.UserNotTicketErr;
                }

                // 非VIP用户限制聊天主播
                if (vipUser == null)
                {
                    // 检验邮票是否已经绑定主播
                    var ticket = tickets.FirstOrDefault(p => p.AnchorId != null && p.AnchorId == anchor.UserId);
                    if (ticket != null)
                    {
                        if (ticket.Category == OrderTypeConfig.UnLimit.ToString())
                            isLimit = false;
                        else
                            remaindCount = (int)ticket.RemainderTicket;
                    }
                    // 未绑定主播时检验是否有充值邮票
                    else
                    {
                        ticket = tickets.FirstOrDefault(p => p.AnchorId == null);
                        if (ticket == null)
                            return SendMessageTips.UserNotTicketErr;
                        // 如果已充值了邮票，那么绑定主播，并且获取剩余邮票数
                        ticket.AnchorId = anchor.UserId;
                        // 如果是无限制邮票类型，则不限制
                        if (ticket.Category == OrderTypeConfig.UnLimit.ToString())
                        {
                            isLimit = false;
                        }
                        else
                        {
                            // 获取配置中的邮票数量
                            var qTicketConfig = from t in context.TB_TicketConfig where t.Key == ticket.Category select t;
                            var ticketConfig = qTicketConfig.SingleOrDefault();
                            if (ticketConfig != null)
                            {
                                remaindCount = (int)ticketConfig.ChatCount;
                            }
                        }
                        context.SaveChanges();
                    }
                    return SendMessageTips.Success;
                }
                else    // 如果是VIP用户不限制聊天主播
                {
                    // 存在无限制类型邮票，则不限制用户发送消息条数
                    var ticket = tickets.FirstOrDefault(p => p.Category == OrderTypeConfig.UnLimit.ToString());
                    if (ticket != null)
                    {
                        isLimit = false;
                    }
                    else
                    {
                        ticket = tickets.FirstOrDefault(p => p.RemainderTicket>0);
                        if (ticket == null)
                            return SendMessageTips.UserNotTicketErr;
                        remaindCount = (int)ticket.RemainderTicket;
                    }
                    return SendMessageTips.Success;
                }
                
            }
        }

        public SendMessageTips RecordChatTicketCount(long userId, string token, long anchorId, int count,out bool isLimit, out int remaindTicket)
        {
            using (var context = base.NewContext())
            {
                isLimit = true;
                remaindTicket = 0;
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return SendMessageTips.UserNotLoginErr;

                var qVipUser = from v in context.TB_UserVIP
                               where v.UserId == userId && v.StartTime <= DateTime.Now && v.EndTime > DateTime.Now
                               select v;
                var vipUser = qVipUser.FirstOrDefault();
                // 查找当前用户邮票
                var qTickets = from t in context.TB_UserTicket
                               where t.UserId == userId
                               select t;
                var tickets = qTickets.ToList();
                if (tickets == null || tickets.Count ==0)
                    return SendMessageTips.Failed;
                
                // VIP用户
                if(vipUser!=null)
                {
                    // 是否存在无限制的邮票
                    var ticket = tickets.FirstOrDefault(p => p.Category == OrderTypeConfig.UnLimit.ToString());
                    if (ticket != null)
                    {
                        isLimit = false;
                        return SendMessageTips.Success;
                    }
                    // 不存在无限制的邮票
                    ticket = tickets.FirstOrDefault(p => p.RemainderTicket > 0);
                    if (ticket == null)
                        return SendMessageTips.Failed;

                    remaindTicket = (int)ticket.RemainderTicket - count;
                    if (remaindTicket <= 0)
                        ticket.RemainderTicket = 0;
                    else
                        ticket.RemainderTicket = remaindTicket;
                    context.SaveChanges();
                }
                else    // 非VIP用户
                {
                    // 如果跟该主播绑定的是无限制邮票
                    var unLimitTicket = tickets.FirstOrDefault(p => p.AnchorId == anchorId && p.Category == OrderTypeConfig.UnLimit.ToString());
                    if (unLimitTicket != null)
                    {
                        isLimit = false;
                        return SendMessageTips.Success;
                    }

                    var limitTicket = tickets.FirstOrDefault(p => p.AnchorId == anchorId && p.Category == OrderTypeConfig.Limit.ToString() && p.RemainderTicket > 0);
                    if(limitTicket == null)
                        return SendMessageTips.Failed;

                    remaindTicket = (int)limitTicket.RemainderTicket - count;
                    if (remaindTicket <= 0)
                        limitTicket.RemainderTicket = 0;
                    else
                        limitTicket.RemainderTicket = remaindTicket;
                    context.SaveChanges();
                }
                return SendMessageTips.Success;
            }
        }

        public int GetRemindMessageCount(long userId, string token, out bool isLimit)
        {
            using (var context = base.NewContext())
            {
                isLimit = true;
                // 主播无限畅聊
                var qAnchor = from a in context.TB_Anchor
                              where a.IsAuth && !a.IsBlack && a.UserId == userId
                              select a;
                if (qAnchor.SingleOrDefault() != null)
                {
                    isLimit = false;
                    return 0;
                }

                // VIP用户无限畅聊
                var qVipUser = from v in context.TB_UserVIP
                               where v.UserId == userId && v.StartTime <= DateTime.Now && v.EndTime > DateTime.Now
                               select v;
                if (qVipUser.FirstOrDefault() != null)
                {
                    isLimit = false;
                    return 0;
                }

                // 普通用户免费发送20次
                var qMessgeLog = from m in context.TB_NeteaseText
                                 where m.FromUserId == userId
                                 select m;
                var count = qMessgeLog.Count();
                var limitCount = TypeHelper.TryParse(ConfigHelper.GetValue("SendMessageLimit"),0);
                if (count >= limitCount)
                    return 0;
                return limitCount-count;
            }
        }
    }
}
