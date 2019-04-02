using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Services.Interface;
using Opcomunity.Services.Helpers;
using Newtonsoft.Json.Linq;
using Opcomunity.Data.Entities;
using Utility.Common;

namespace Opcomunity.Services.Implementations
{
    public class NeteaseService : ServiceBase, INeteaseService
    {
        public void RollbackLiveCallRecord(long channelId, int status, string statusDescription)
        {
            using (var context = base.NewContext())
            {
                var model = context.TB_NeteaseCall.SingleOrDefault(p => p.ChannelId == channelId);
                if (model != null)
                {
                    model.Status = status;
                    model.StatusDescription = statusDescription;
                }
                context.SaveChanges();
            }
        }

        public NeteaseCallNotifyTips SaveLiveCallRecord(string md5, string curTime, string checkSum, string requestBody)
        {
            #region 验证回调参数
            if (!NeteaseCore.CheckNotifyRequest(md5, curTime, checkSum, requestBody))
                return NeteaseCallNotifyTips.MD5OrCheckSumErr;
            Log4NetHelper.Info(log,"1、验证参数成功");

            JObject jObject = JObject.Parse(requestBody);
            if (jObject == null)
                return NeteaseCallNotifyTips.RequestBodyErr;

            Log4NetHelper.Info(log, "2、请求内容为有效json数据");
            int eventType = TypeHelper.TryParse(jObject["eventType"].ToString(), 0);
            #region 处理聊天消息
            if (eventType == 1)
            {
                var msgType = TypeHelper.TryParse(jObject["msgType"].ToString(), "");
                if(msgType == "TEXT")
                {
                    #region 请求消息体
                    //{
                    //    "fromNick": "香水有毒",
                    //      "msgType": "TEXT",
                    //      "msgidServer": "29019995240",
                    //      "fromAccount": "457ad7a94f23df4bdf8fdddd867859d4",
                    //      "fromClientType": "AOS",
                    //      "fromDeviceId": "dd0f0fde-f79a-41c4-ad62-2076d2cb5194",
                    //      "eventType": "1",
                    //      "body": "好吧",
                    //      "convType": "PERSON",
                    //      "msgidClient": "d2b28b72b4e8487dbe3b9e931c7fa618",
                    //      "resendFlag": "0",
                    //      "msgTimestamp": "1529943238808",
                    //      "to": "4f1fd6b6dd246c08933e569b9c84cd1e"
                    //}

                    using (var context = base.NewContext())
                    {
                        var fromAccount = jObject["fromAccount"].ToString();
                        var toAccount = jObject["to"].ToString();

                        long fromUserId = 0, toUserId = 0;

                        var fromModel = context.TB_NeteaseAccount.SingleOrDefault(p => p.NeteaseAccId == fromAccount);
                        var toModel = context.TB_NeteaseAccount.SingleOrDefault(p => p.NeteaseAccId == toAccount);
                        if (fromModel != null)
                            fromUserId = fromModel.UserId;
                        if (toModel != null)
                            toUserId = toModel.UserId;

                        #endregion
                        TB_NeteaseText model = new TB_NeteaseText()
                        {
                            EventType = eventType,
                            MsgType = jObject["msgType"].ToString(),
                            FromNick = jObject["fromNick"].ToString(),
                            MsgIdServer = jObject["msgidServer"].ToString(),
                            FromAccount = fromAccount,
                            FromUserId = fromUserId,
                            ToAccount = toAccount,
                            ToUserId = toUserId,
                            FromClientType = jObject["fromClientType"].ToString(),
                            FromDeviceId = jObject["fromDeviceId"].ToString(),
                            Body = jObject["body"].ToString(),
                            ConvType = jObject["convType"].ToString(),
                            MsgIdClient = jObject["msgidClient"].ToString(),
                            ResendFlag = TypeHelper.TryParse(jObject["resendFlag"].ToString(), 0) != 0,
                            MsgTimestamp = TypeHelper.TryParse(jObject["msgTimestamp"].ToString(), 0L),
                            CreateTime = DateTime.Now
                        };
                        context.TB_NeteaseText.Add(model);
                        context.SaveChanges();
                        Log4NetHelper.Info(log, "3、存储聊天记录成功");
                        return NeteaseCallNotifyTips.Success;
                    }
                }
                return NeteaseCallNotifyTips.EventTypeIsNotFive;
            } 
            #endregion
            #region 处理视频通话
            else if (eventType == 5)
            {
                #region Request Body参数示例
                /* 接通的情况
                    {
                      "duration": "102",
                      "ext": "100000|女神|http://st.opcomunity.com/images/avatar/1801/U10000030152138.jpg",
                      "createtime": "1517647416584",
                      "members": "[{\"duration\":51,\"accid\":\"b89623616501e1dda7ab1be8bee83221\"},{\"duration\":51,\"caller\":true,\"accid\":\"287de128b3261f672d5b9b1d4c603cb4\"}]",
                      "eventType": "5",
                      "type": "VEDIO",
                      "channelId": "6216283818279222",
                      "live": "0",
                      "status": "SUCCESS"
                    } 

                    未接通情况
                    {
                      "duration": "0",
                      "ext": "100000|女神|http://st.opcomunity.com/images/avatar/1801/U10000030152138.jpg",
                      "createtime": "1517647225056",
                      "members": "[{\"duration\":0,\"caller\":true,\"accid\":\"287de128b3261f672d5b9b1d4c603cb4\"}]",
                      "eventType": "5",
                      "type": "VEDIO",
                      "channelId": "6216283033772313",
                      "live": "0",
                      "status": "SINGLE_PARTICIPATE"
                    }
                    */
                #endregion
                string members = TypeHelper.TryParse(jObject["members"].ToString(), "");
                JArray jArray = JArray.Parse(members);
                if (jArray.Count != 2)
                    return NeteaseCallNotifyTips.LiveCallTimeIsZero;
                #endregion

                Log4NetHelper.Info(log, "3、视频通话参数为有效参数");
                using (var context = base.NewContext())
                {
                    #region 解析回调参数
                    var curChannelId = TypeHelper.TryParse(jObject["channelId"].ToString(), 0L);
                    // 双方通话时长总和
                    int totalDuration = TypeHelper.TryParse(jObject["duration"].ToString(), 0);
                    List<TB_NeteaseCallMember> listMembers = new List<TB_NeteaseCallMember>();
                    int callRatio = 0, totalFee = 0;
                    long userId = 0, anchorId = 0;
                    // 根据回调的参数判断主播和用户
                    for (var i = 0; i < jArray.Count; i++)
                    {
                        string accId = TypeHelper.TryParse(jArray[i]["accid"].ToString(), "");
                        // 根据网易云账号映射系统内用户账户
                        var neteaseAccount = context.TB_NeteaseAccount.SingleOrDefault(n => n.NeteaseAccId == accId);
                        if (neteaseAccount == null)
                            return NeteaseCallNotifyTips.NeteaseAccountIsNotExistErr;
                        bool isCaller = false;
                        try { isCaller = Convert.ToBoolean(jArray[i]["caller"].ToString()); }
                        catch { isCaller = false; }

                        neteaseAccount.ChatStatus = (int)NeteaseChatStatusConfig.Free;
                        if (isCaller) { userId = neteaseAccount.UserId; }
                        else
                        {
                            var curAnchorId = neteaseAccount.UserId;
                            var anchor = context.TB_Anchor.SingleOrDefault(a => a.UserId == neteaseAccount.UserId);
                            if (anchor == null)
                            {
                                context.SaveChanges();
                                return NeteaseCallNotifyTips.AnchorAccountIsNotExistErr;
                            }
                            anchorId = curAnchorId;
                            callRatio = anchor.CallRatio;
                            // 网易云回调的参数中把双方的时长和作为总时长，本系统内只计单边通话时长
                            int totalSecond = totalDuration / 2;
                            // 计算总呼叫费用
                            totalFee = (int)Math.Ceiling(totalSecond / 60.0) * callRatio;
                        }

                        listMembers.Add(new TB_NeteaseCallMember
                        {
                            ChannelId = TypeHelper.TryParse(jObject["channelId"].ToString(), 0L),
                            AccId = TypeHelper.TryParse(jArray[i]["accid"].ToString(), ""),
                            Duration = TypeHelper.TryParse(jArray[i]["duration"].ToString(), 0),
                            IsCaller = isCaller
                        });
                    }
                    #endregion

                    // 验证通话记录是否存在，避免重复记录
                    var qCall = context.TB_NeteaseCall.SingleOrDefault(c => c.ChannelId == curChannelId);
                    if (qCall != null)
                    {
                        context.SaveChanges();
                        return NeteaseCallNotifyTips.RepeatSendLiveRecord;
                    }

                    Log4NetHelper.Info(log, "4、视频通话记录在数据库中不存在");
                    #region 验证用户金额并获取实际交易金额
                    // 实际交易金额
                    int actualTransferFee = totalFee;
                    // 验证用户账户余额是否大于0
                    var qUserCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId && p.CurrentCoin > 0);
                    if (qUserCoin == null)
                    {
                        context.SaveChanges();
                        return NeteaseCallNotifyTips.UserCoinNotEnoughErr;
                    }

                    Log4NetHelper.Info(log, "5、用户余额验证通过");

                    // 当用户的账户余额不足够支付本次交易时，以用户实际拥有的账户余额计费
                    if (qUserCoin.CurrentCoin - totalFee <= 0)
                        actualTransferFee = (int)qUserCoin.CurrentCoin;
                    #endregion

                    #region 保存通话记录
                    TB_NeteaseCall call = new TB_NeteaseCall
                    {
                        ChannelId = curChannelId,
                        CallerId = userId,
                        AnchorId = anchorId,
                        CallRatio = callRatio,
                        TotalFee = totalFee,
                        ActualTransferFee = actualTransferFee,
                        TotalDuration = totalDuration,
                        Duration = totalDuration / 2,
                        Ext = TypeHelper.TryParse(jObject["ext"].ToString(), ""),
                        EventType = eventType,
                        CallTime = TypeHelper.TryParse(jObject["createtime"].ToString(), 0L),
                        Type = TypeHelper.TryParse(jObject["type"].ToString(), ""),
                        Live = TypeHelper.TryParse(jObject["live"].ToString(), 0),
                        CallStatus = TypeHelper.TryParse(jObject["status"].ToString(), ""),
                        Status = (int)ExecuteStatusConfig.Success,
                        StatusDescription = ExecuteStatusConfig.Success.GetRemark(),
                        CreateTime = DateTime.Now
                    };
                    context.TB_NeteaseCall.Add(call);
                    context.TB_NeteaseCallMember.AddRange(listMembers);
                    #endregion

                    #region 扣除用户交易金额，并记录消费流水
                    qUserCoin.CurrentCoin -= actualTransferFee;
                    var userCoinJournal = new TB_UserCoinJournal()
                    {
                        UserId = userId,
                        CoinCount = actualTransferFee,
                        CurrentCount = qUserCoin.CurrentCoin,
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

                    Log4NetHelper.Info(log, "6、视频通话记录成功");
                    context.SaveChanges();
                    return NeteaseCallNotifyTips.Success;
                }
            } 
            #endregion
            else
                return NeteaseCallNotifyTips.EventTypeIsNotFive;
        }

        public void UpdateAnchorGlamour(long anchorId, long glamour)
        {
            using (var context = base.NewContext())
            {
                var query = from a in context.TB_Anchor
                            where a.UserId == anchorId && a.IsAuth && !a.IsBlack
                            select a;
                var anchor = query.SingleOrDefault();
                if (anchor != null)
                {
                    anchor.Glamour += glamour;
                    context.SaveChanges();
                }
            }
        }
    }
}
