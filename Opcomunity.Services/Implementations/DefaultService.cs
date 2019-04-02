using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.SqlClient;
using Opcomunity.Services.Dtos;
using System.Data.Entity;
using Utility.Common;

namespace Opcomunity.Services.Implementations
{
    public class DefaultService :ServiceBase, IDefaultService
    {
        public void DataInitialize()
        {
            using (var context = base.NewContext())
            {
                if (context.TB_Topic.Any())
                    return;

                TB_Topic[] topics = new TB_Topic[] {
                    new TB_Topic{
                        UserId = 100000,
                        Description = "好美啊",
                        Price = 125,
                        Discount = 100,
                        Collects = 99,
                        Comments = 17,
                        Views = 100000,
                        IsAvailable = true,
                        CreateTime =DateTime.Now,
                        UpdateTime = DateTime.Now
                    },
                    new TB_Topic{
                        UserId = 100000,
                        Description = "我上传的",
                        Price = 100,
                        Discount = 60,
                        Collects = 443,
                        Comments = 23,
                        Views = 143,
                        IsAvailable = true,
                        CreateTime =DateTime.Now,
                        UpdateTime = DateTime.Now
                    },
                    new TB_Topic{
                        UserId = 100000,
                        Description = "Ok好棒",
                        Price = 300,
                        Discount = 50,
                        Collects = 22,
                        Comments = 12,
                        Views = 149,
                        IsAvailable = true,
                        CreateTime =DateTime.Now,
                        UpdateTime = DateTime.Now
                    },
                    new TB_Topic{
                        UserId = 100001,
                        Description = "哈哈哈",
                        Price = 125,
                        Discount = 100,
                        Collects = 21,
                        Comments = 32,
                        Views = 54,
                        IsAvailable = true,
                        CreateTime =DateTime.Now,
                        UpdateTime = DateTime.Now
                    }
                };
                foreach (var item in topics)
                {
                    context.TB_Topic.Add(item);
                }
                context.SaveChanges();

                for(int id = 1000; id<=1003; id++)
                {
                    for(int index = 0; index<=4; index++)
                    {
                        context.TB_OssObject.Add(
                            new TB_OssObject
                            {
                                TopicId = id,
                                Bucket = "test"+(new Random()).Next(10000,999999),
                                Ext = ".jpg",
                                FileSize = 125,
                                HashValue = "MM" + (new Random()).Next(10000, 999999),
                                IsLock = true,
                                MimeType = "jpg/jpeg",
                                OssKey = "M" + (new Random()).Next(10000, 999999),
                                IsAvailable = true,
                                SortId = index,
                                CreateTime = DateTime.Now.AddMinutes((new Random()).Next(10, 50)),
                                UpdateTime = DateTime.Now
                            }
                        );
                    }
                    context.SaveChanges();
                }
            }
        }

        public List<GiftItem> GetGifts()
        {
            using (var context = base.NewContext())
            {
                var query = from g in context.TB_Gift
                            where g.IsAvailable
                            orderby g.SortId
                            select new GiftItem
                            {
                                Id = g.Id,
                                Name = g.Name,
                                Conver = g.Conver,
                                Description = g.Description,
                                OriginalPrice = g.OriginalPrice,
                                DiscountPrice = (g.IsDiscount && g.DiscountStart <= DateTime.Now && g.DiscountEnd >= DateTime.Now && g.Discount!=null)?
                                                (g.OriginalPrice * g.Discount ?? 0) / 100: g.OriginalPrice
                            };
                return query.ToList();
            }
        }

        public List<MessageItem> GetMessage(long userId, string token, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                bool isLogin = false;
                if(userId > 0 && !string.IsNullOrEmpty(token))
                {
                    var qUser = from userInfo in context.TB_User
                                join tokenInfo in context.TB_UserTokenInfo
                                on userInfo.Id equals tokenInfo.UserId
                                where userInfo.Id == userId && tokenInfo.UserToken == token
                                select userInfo;
                    if (qUser.FirstOrDefault() != null)
                        isLogin = true;
                }
                
                // 登录用户
                if (isLogin)
                {
                    var query = from m in context.TB_Message
                                where m.IsAvailable && (m.UserId ==null || (m.UserId!=null && m.UserId == userId))
                                && m.StartTime <= DateTime.Now && m.EndTime >= DateTime.Now
                                orderby m.SortId, m.UpdateTime descending
                                select new MessageItem
                                {
                                    Id = m.Id,
                                    Category = m.Category,
                                    CategoryDescription = m.CategoryDescription,
                                    Title = m.Title,
                                    Content = m.Content,
                                    Parameters = m.Parameters,
                                    CrreateTime = DbFunctions.DiffSeconds(new DateTime(1970, 1, 1), m.CreateTime)??0
                                };
                    return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
                // 非登录用户
                else
                {
                    var query = from m in context.TB_Message
                                where m.IsAvailable && m.UserId == null
                                && m.StartTime <= DateTime.Now && m.EndTime >= DateTime.Now
                                orderby m.SortId, m.UpdateTime descending
                                select new MessageItem
                                {
                                    Id = m.Id,
                                    Category = m.Category,
                                    CategoryDescription = m.CategoryDescription,
                                    Title = m.Title,
                                    Content = m.Content,
                                    Parameters = m.Parameters,
                                    CrreateTime = DbFunctions.DiffSeconds(new DateTime(1970, 1, 1), m.CreateTime) ?? 0
                                };
                    return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
            }
        }

        public List<TipOffCategoryItem> GetTipOffCategory()
        {
            using (var context = base.NewContext())
            {
                var query = from c in context.TB_TipOffCategory
                            where c.IsAvailable
                            orderby c.SortId
                            select new TipOffCategoryItem {
                                Id = c.Id,
                                Description = c.Description
                            };
                return query.ToList();
            }
        }

        public int SaveFeedBack(long userId, string token, string description)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.FirstOrDefault() == null)
                    return 2;

                var model = new TB_FeedBack()
                {
                    UserId = userId,
                    Status = (int)ProcessStatusConfig.WaitForProcess,
                    StatusDescription = ProcessStatusConfig.WaitForProcess.GetRemark(),
                    Description = description,
                    FeedBackTime = DateTime.Now,
                    Ip = WebUtils.GetClientIP()
                };
                context.TB_FeedBack.Add(model);
                return context.SaveChanges();
            }
        }

        public int SaveTipOff(long userId, string token, long anchorId, int categoryId, string description)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.FirstOrDefault() == null)
                    return 2;

                var model = new TB_TipOff()
                {
                    UserId = userId,
                    AnchorId = anchorId,
                    Status = (int)ProcessStatusConfig.WaitForProcess,
                    StatusDescription = ProcessStatusConfig.WaitForProcess.GetRemark(),
                    Description = description,
                    CategoryId = categoryId,
                    CreateTime = DateTime.Now,
                    Ip = WebUtils.GetClientIP()
                };
                context.TB_TipOff.Add(model);
                return context.SaveChanges();
            }
        }

        public List<InviteRewardItem> GetInviteRewardRankList(int pageIndex, int pageSize, out BasicTips tips)
        {
            using (var context = base.NewContext())
            {
                var qInviteReward = from income in context.TB_UserIncomeJournal.Where(p => p.JournalType == (int)CoinJournalConfig.InviteReward)
                                    join i in context.TB_UserInvite
                                    on income.OriginUserId equals i.NewUserId
                                    group income by new { UserId = income.UserId }
                                    into jg
                                    select new { UserId = jg.Select(p => p.UserId).FirstOrDefault(), TotalReward = jg.Sum(p => p.IncomeCount) }
                                    into t
                                    join u in context.TB_User on t.UserId equals u.Id
                                    select new InviteRewardItem
                                    {
                                        UserId = t.UserId,
                                        NickName = u.NickName,
                                        Avatar = u.Avatar,
                                        ThumbnailAvatar = u.ThumbnailAvatar,
                                        TotalReward = t.TotalReward
                                    };

                tips = BasicTips.Success;
                return qInviteReward.OrderByDescending(p => p.TotalReward).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public TB_AppVersion CheckAppUpdate(int channel)
        {
            using (var context = base.NewContext())
            {
                var maxVersion = context.TB_AppVersion.Where(p => p.Channel == channel).Max(p => p.Version);
                if(!string.IsNullOrEmpty(maxVersion))
                    return context.TB_AppVersion.FirstOrDefault(p => p.Channel == channel && p.Version == maxVersion);
                return null;
            }
        }
        
        public List<TB_StatisticsChannel> GetChannelStatistics(int channel, DateTime startDate, DateTime endDate)
        {
            using (var context = base.NewContext())
            {
                var query = from s in context.TB_StatisticsChannel
                              where s.Channel == channel
                              && DbFunctions.DiffDays(s.Date, startDate) <= 0
                              && DbFunctions.DiffDays(s.Date, endDate) >= 0
                              select s;
                return query.ToList();
            }
        }
    }
}
