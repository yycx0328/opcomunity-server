using Newtonsoft.Json.Linq;
using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Helpers;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility.Common;

namespace Opcomunity.Services.Implementations
{
    public class AnchorService: ServiceBase, IAnchorService
    {
        public List<BannerItem> GetBannerList(int top)
        {
            using (var context = base.NewContext())
            {
                var query = from b in context.TB_Banner
                            where b.StartTime <= DateTime.Now && b.EndTime >= DateTime.Now
                            && b.IsAvailable
                            orderby b.SortId descending
                            select new BannerItem {
                                Id = b.Id,
                                Title = b.Title,
                                Category = b.Category,
                                Description = b.Description,
                                Image = b.Image,
                                Link = b.Link,
                                Parameters = b.Parameters,
                                SortId = b.SortId
                            };
                return query.Take(top).ToList();
            }
        }

        public List<AnchorItem> GetRecormmendAnchorList(int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from a in context.TB_Anchor
                            join u in context.TB_User
                            on a.UserId equals u.Id
                            join n in context.TB_NeteaseAccount
                            on a.UserId equals n.UserId
                            into an 
                            from n in an.DefaultIfEmpty()
                            where a.IsAuth && !a.IsBlack
                            orderby n.ChatStatus descending, a.Glamour descending, a.UserId descending
                            select new AnchorItem {
                                AnchorId = a.UserId,
                                Name = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Glamour = a.Glamour,
                                CallRatio = a.CallRatio,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                AuthTime = a.AuthTime,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus
                            };
                return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public List<TB_AnchorCategory> GetAnchorCategoryList()
        {
            using (var context = base.NewContext())
            {
                var query = from c in context.TB_AnchorCategory
                            where c.IsAvailable
                            orderby c.SortId
                            select c;
                return query.ToList();
            }
        }

        public List<AnchorItem> GetDiscoverAnchorList(int category, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from a in context.TB_Anchor
                            join u in context.TB_User
                            on a.UserId equals u.Id
                            join c in context.TB_AnchorCategoryRelation
                            on a.UserId equals c.AnchorId
                            join n in context.TB_NeteaseAccount
                            on a.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            where a.IsAuth && !a.IsBlack && c.CategoryId == category
                            select new AnchorItem
                            {
                                AnchorId = a.UserId,
                                Name = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Glamour = a.Glamour,
                                CallRatio = a.CallRatio,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                AuthTime = a.AuthTime,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus
                            };
                return query.OrderByDescending(p => p.NeteaseChatStatus).ThenByDescending(p=>p.Glamour).ThenByDescending(p => p.AnchorId).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public AnchorDetailInfoItem GetAnchorDetailInfo(long userId, string token, long anchorId)
        {
            using (var context = base.NewContext())
            {
                bool isLogin = false;
                if (userId >= 0&& !string.IsNullOrEmpty(token))
                {
                    var qUser = from userInfo in context.TB_User
                                join tokenInfo in context.TB_UserTokenInfo
                                on userInfo.Id equals tokenInfo.UserId
                                where userInfo.Id == userId && tokenInfo.UserToken == token
                                select userInfo;
                    if (qUser.FirstOrDefault() != null)
                        isLogin = true;
                }

                var qAnchor = from a in context.TB_Anchor
                              where a.IsAuth && !a.IsBlack && a.UserId == anchorId
                              select a;
                var anchor = qAnchor.SingleOrDefault();
                if (anchor == null)
                    return null;

                var query = from u in context.TB_User
                            join n in context.TB_NeteaseAccount
                            on u.Id equals n.UserId
                            into un
                            from n in un.DefaultIfEmpty()
                            where u.Id == anchorId
                            select new AnchorDetailInfoItem
                            {
                                UserId = u.Id,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Birthday = u.Birthday,
                                Height = u.Height,
                                Weight = u.Weight,
                                IsAnchor = true,
                                Constellation = u.Constellation, 
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                CallRatio = anchor.CallRatio, 
                                Glamour = anchor.Glamour,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId, 
                                NeteaseLoginStatus = n == null ? 0 : n.LoginStatus,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                IsFollow = !isLogin? false : (from f in context.TB_UserFollow
                                                              where f.UserId == userId && f.FollowedUserId == anchorId
                                                              select f).FirstOrDefault() != null,
                                FollowCount = (from f in context.TB_UserFollow
                                               where f.FollowedUserId == anchorId
                                               select f).Count(),
                                UserPhotoItems= from up in context.TB_UserPhoto
                                                where up.UserId == anchorId
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

        private bool IsFollow(long userId, long anchorId)
        {
            using (var context = base.NewContext())
            {
                var query = from f in context.TB_UserFollow
                            where f.UserId == userId && f.FollowedUserId == anchorId
                            select f;
                if (query.SingleOrDefault() != null)
                    return true;
                return false;
            }
        }

        private int FollowCount(long anchorId)
        {
            using (var context = base.NewContext())
            {
                var query = from f in context.TB_UserFollow
                            where f.FollowedUserId == anchorId
                            select f;
                var count = query.Count();
                return count;
            }
        }

        public TB_User GetLoginUserInfo(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                var user = query.SingleOrDefault();
                if (user != null)
                    return user;
                return null;
            }
        }

        public bool IsAnchorExist(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            join anchor in context.TB_Anchor
                            on userInfo.Id equals anchor.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                return query.Count() >= 1;
            }
        }

        public AplayTobeAnchorTips SaveAnchor(TB_Anchor model)
        {
            using (var context = base.NewContext())
            {
                var anchor = context.TB_Anchor.SingleOrDefault(p => p.UserId == model.UserId);
                if (anchor != null)
                {
                    if(anchor.IsAuth)
                        return AplayTobeAnchorTips.AlreadyAuthAnchorErr;

                    if (anchor.IsBlack)
                        return AplayTobeAnchorTips.ForbiddenTobeAnchor;

                    anchor.ApplyTime = model.ApplyTime;
                    anchor.Description = model.Description;
                    context.SaveChanges();
                    return AplayTobeAnchorTips.Success;
                }

                context.TB_Anchor.Add(model);
                context.SaveChanges();
                return AplayTobeAnchorTips.Success;
            }
        }

        public bool SaveAnchorIdentity(TB_AnchorIdentity model)
        {
            using (var context = base.NewContext())
            {
                var identity = context.TB_AnchorIdentity.SingleOrDefault(p => p.UserId == model.UserId);
                if (identity == null)
                {
                    context.TB_AnchorIdentity.Add(model);
                }
                else
                {
                    identity.IdentityPositive = model.IdentityPositive;
                    identity.IdentityOpposite = model.IdentityOpposite;
                }
                return context.SaveChanges()>0;
            }
        }
        
        public List<DevoteRankItem> GetDevoteRank(long anchorId)
        {
            using (var context = base.NewContext())
            {
                var query = from s in (
                                from t in (
                                    (from c in context.TB_NeteaseCall where c.AnchorId == anchorId && c.Status == 0
                                        select new { UserId = c.CallerId, Devote = c.ActualTransferFee })
                                    .Concat(from g in context.TB_GiftTransaction where g.AnchorId == anchorId && g.Status == 0
                                        select new { UserId = g.UserId, Devote = g.CostPrice })
                                ) group t by t.UserId into cg
                                select new { UserId = cg.Key,TotalDevote = cg.Sum(p=>p.Devote) }
                            )
                            join u in context.TB_User on s.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                            on s.UserId equals a.UserId
                            into aa
                            from a in aa.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on u.Id equals v.UserId
                            into t
                            from v in t.DefaultIfEmpty()
                            orderby s.TotalDevote descending
                            select new DevoteRankItem {
                                IsAnchor = a!=null,
                                UserId = s.UserId,
                                UserName = u.NickName,
                                UserAvatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                IsVip = v!=null,
                                TotalDevote = s.TotalDevote
                            };  
                return query.Take(30).ToList();
            }
        }

        public BasicTips SetCallRate(long userId, string token,int callRatio)
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

                var qAnchor = from a in context.TB_Anchor where a.UserId == userId select a;
                var anchor = qAnchor.SingleOrDefault();
                if (anchor == null)
                    return BasicTips.UserNotAnchor;

                anchor.CallRatio = callRatio;
                context.SaveChanges();
                return BasicTips.Success;
            }
        }

        public AnchorItem GetRandomAnchor()
        {
            using (var context = base.NewContext())
            {
                var query = from a in context.TB_Anchor
                            join u in context.TB_User
                            on a.UserId equals u.Id
                            join n in context.TB_NeteaseAccount
                            on a.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            where a.IsAuth && !a.IsBlack && n.ChatStatus==(int)NeteaseChatStatusConfig.OffLine
                            orderby Guid.NewGuid()
                            select new AnchorItem
                            {
                                AnchorId = a.UserId,
                                Name = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Glamour = a.Glamour,
                                CallRatio = a.CallRatio,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                AuthTime = a.AuthTime,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus
                            };
                return query.Take(1).FirstOrDefault();
            }
        }

        public SendBatchMessageTips SendBatchMsg(long userId, string token, string message)
        {
            if (userId <= 0 || string.IsNullOrEmpty(token) || string.IsNullOrEmpty(message))
                return SendBatchMessageTips.ParameterErr;

            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() == null)
                    return SendBatchMessageTips.UserNotLoginErr;

                var qAnchorAccId = from a in context.TB_Anchor
                              join n in context.TB_NeteaseAccount
                              on a.UserId equals n.UserId
                              where a.IsAuth && !a.IsBlack && a.UserId == userId select n.NeteaseAccId;
                var anchorAccId = qAnchorAccId.SingleOrDefault();
                if (string.IsNullOrEmpty(anchorAccId))
                    return SendBatchMessageTips.UserNotAnchor;

                // 主播10分钟内只能发送一条群发消息
                var qSendLog = from s in context.TB_SendBatchMessage
                               where s.UserId == userId && DbFunctions.DiffSeconds(s.CreateTime , DateTime.Now) < 600
                               select s;
                if (qSendLog.FirstOrDefault() != null)
                    return SendBatchMessageTips.TooFrequentlyErr;

                string jMessage = "{\"msg\":\""+ message + "\"}";

                // 对一小时内在线的前500名非主播用户发送消息
                var qToAccIds = (from v in context.TB_AppVisitLog.Where(p=> !context.TB_Anchor.Select(m=>m.UserId).Contains(p.UserId))
                                 join n in context.TB_NeteaseAccount
                                 on v.UserId equals n.UserId
                                 where DbFunctions.DiffSeconds(v.CreateTime, DateTime.Now) < 3600
                                 select n.NeteaseAccId).Distinct();

                var toAccIdList = qToAccIds.Take(500).ToList();
                if (toAccIdList == null || toAccIdList.Count == 0)
                    return SendBatchMessageTips.Success;

                var jArray = JArray.FromObject(toAccIdList);
                string toAccIds = jArray.ToString();
                Log4NetHelper.Info(log, "ToAccIds:"+ toAccIds);

                NameValueCollection data = new NameValueCollection();
                data.Add("fromAccid", anchorAccId);
                data.Add("toAccids", toAccIds);
                data.Add("type", "0");
                data.Add("body", jMessage);
                string result = NeteaseCore.PostNeteaseRequest(NeteaseRequestActionConfig.SEND_BATCH_MSG, data);
                if (string.IsNullOrEmpty(result))
                    return SendBatchMessageTips.MessageSendFaildErr;

                JObject jobject = JObject.Parse(result);
                if (jobject == null || jobject["code"].ToString() != "200")
                    return SendBatchMessageTips.MessageSendFaildErr;

                TB_SendBatchMessage sendMessage = new TB_SendBatchMessage()
                {
                    UserId = userId,
                    Message = jMessage,
                    SendCount = toAccIdList.Count,
                    CreateTime =DateTime.Now
                };
                context.TB_SendBatchMessage.Add(sendMessage);
                context.SaveChanges();
                return SendBatchMessageTips.Success;
            }
        }

    }
}
