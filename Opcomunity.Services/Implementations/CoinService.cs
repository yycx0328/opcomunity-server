using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using Utility.Common;

namespace Opcomunity.Services.Implementations
{
    public class CoinService : ServiceBase, ICoinService
    {

        public void ModifyNetease()
        {
            using (var context = base.NewContext())
            {
                var query = from u in context.TB_User
                            join n in context.TB_NeteaseAccount
                            on u.Id equals n.UserId
                            select new NeteaseUpdateModel
                            {
                                AccId = n.NeteaseAccId,
                                Icon = u.Avatar,
                                Name = u.NickName
                            };

                var list = query.ToList();
                int index = 0;
                foreach(var item in list)
                {
                    index++;
                    NameValueCollection data = new NameValueCollection();
                    data.Add("accid", item.AccId);
                    data.Add("icon", item.Icon);
                    data.Add("name", item.Name);
                    string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.CRT_UPDATEUSER_URL, data);
                    Log4NetHelper.Info(log, string.Format("更新第{0}条网易云账号{1}昵称{2}，结果：{3}", index, item.AccId,item.Name, result));
                    Thread.Sleep(1000);
                }
            }
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

        public CashOutTips CoinSingleOut(long userId, int outCount, CoinJournalConfig outJournal, out long currentCoinCount)
        {
            using (var context = base.NewContext())
            {
                currentCoinCount = 0;
                var qUserCoin = context.TB_UserCoin.SingleOrDefault(p => p.UserId == userId && p.CurrentCoin >= outCount);
                if (qUserCoin == null)
                    return CashOutTips.UserCoinNotEnoughErr;
                qUserCoin.CurrentCoin -= outCount;
                var userCoinJournal = new TB_UserCoinJournal()
                {
                    UserId = userId,
                    CoinCount = outCount,
                    CurrentCount = qUserCoin.CurrentCoin,
                    IOStatus = CoinIOStatusConfig.O.ToString(),
                    JournalType = (int)outJournal,
                    JournalDesc = outJournal.GetRemark(),
                    Ip = WebUtils.GetClientIP(),
                    CreateTime = DateTime.Now
                };
                context.TB_UserCoinJournal.Add(userCoinJournal);

                context.SaveChanges();
                currentCoinCount = qUserCoin.CurrentCoin;
                return CashOutTips.Success;
            }
        }

        public List<CoinJournalItem> GetCoinTransactionRecord(long userId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from j in context.TB_UserCoinJournal
                            where j.UserId == userId
                            orderby j.CreateTime descending
                            select new CoinJournalItem {
                                Id = j.Id,
                                CoinCount = j.CoinCount,
                                CurrentCount = j.CurrentCount,
                                IOStatus = j.IOStatus,
                                JournalType = j.JournalType,
                                JournalDesc = j.JournalDesc,
                                CreateTime = j.CreateTime
                            };
                return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public List<CoinJournalItem> GetCoinIncomeRecord(long userId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from j in context.TB_UserIncomeJournal
                            where j.UserId == userId
                            orderby j.CreateTime descending
                            select new CoinJournalItem
                            {
                                Id = j.Id,
                                CoinCount = j.IncomeCount,
                                CurrentCount = j.CurrentCount,
                                IOStatus = j.IOStatus,
                                JournalType = j.JournalType,
                                JournalDesc = j.JournalDesc,
                                CreateTime = j.CreateTime
                            };
                return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }
    }
}
