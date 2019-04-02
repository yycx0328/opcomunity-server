using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Implementations
{
    public class FollowService : ServiceBase, IFollowService
    {
        public bool IsLoginUser(long userId, string token)
        {
            using (var context = base.NewContext())
            {
                var query = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (query.SingleOrDefault() != null)
                    return true;
                return false;
            }
        }

        public List<FollowUserItem> GetFollowUsers(long userId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            { 
                var query = from uf in context.TB_UserFollow
                            join u in context.TB_User
                            on uf.FollowedUserId equals u.Id
                            join a in context.TB_Anchor
                            on uf.FollowedUserId equals a.UserId
                            join n in context.TB_NeteaseAccount
                            on uf.FollowedUserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on uf.FollowedUserId equals v.UserId
                            into g
                            from v in g.DefaultIfEmpty()
                            where uf.UserId == userId && a.IsAuth && !a.IsBlack
                            orderby uf.FollowTime descending
                            select new FollowUserItem
                            {
                                UserId = uf.FollowedUserId,
                                UserName = u.NickName,
                                UserAvatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                Glamour = a== null?0: a.Glamour,
                                IsAnchor = a !=null,
                                NeteaseChatStatus = n == null ? 0: n.ChatStatus,
                                IsVip = v!=null,
                                FollowTime = uf.FollowTime,
                                UserPhotoItems = (
                                    from ph in context.TB_UserPhoto
                                    where ph.UserId == uf.FollowedUserId
                                    orderby ph.SortId
                                    select new UserPhotoItem
                                    {
                                        SortId = ph.SortId,
                                        ImageWebPath = ph.ImageWebPath,
                                        ThumbnailPath = ph.ThumbnailPath
                                    }
                                )
                            };
                return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public FollowTips FollowUser(long userId, long followedUserId)
        {
            if (userId == followedUserId)
                return FollowTips.CannotFollowSelfErr;
            using (var context = base.NewContext())
            {
                var query = from uf in context.TB_UserFollow
                            where uf.UserId == userId && uf.FollowedUserId == followedUserId
                            select uf;
                if (query.Count() >= 1)
                    return FollowTips.AlreadyFollowErr;

                var queryAnchor = from a in context.TB_Anchor where a.UserId == followedUserId select a;
                if (queryAnchor.Count() == 0)
                    return FollowTips.NotAnchorAccountErr;

                TB_UserFollow follow = new TB_UserFollow()
                {
                    UserId = userId,
                    FollowedUserId = followedUserId,
                    FollowTime = DateTime.Now
                };
                context.TB_UserFollow.Add(follow);
                int result = context.SaveChanges();
                if (result == 1)
                    return FollowTips.Success;
                else
                    return FollowTips.FollowFaild;
            }
        }

        public FollowTips CancelFollow(long userId, long followedUserId)
        {
            using (var context = base.NewContext())
            {
                var query = from uf in context.TB_UserFollow
                            where uf.UserId == userId && uf.FollowedUserId == followedUserId
                            select uf;
                if (query.Count() == 0)
                    return FollowTips.UnFollowErr;

                context.TB_UserFollow.RemoveRange(query);
                int result = context.SaveChanges();
                if (result > 0)
                    return FollowTips.Success;
                else
                    return FollowTips.CancelFollowFaild;
            }
        }

        /// <summary>
        /// 我的粉丝
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<FollowUserItem> GetFollowMineUsers(long userId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from uf in context.TB_UserFollow
                            join u in context.TB_User
                            on uf.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p=> p.IsAuth && !p.IsBlack)
                            on uf.UserId equals a.UserId
                            into aa 
                            from a in aa.DefaultIfEmpty()
                            join n in context.TB_NeteaseAccount
                            on uf.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            join v in context.TB_UserVIP.Where(p => p.StartTime <= DateTime.Now && p.EndTime > DateTime.Now)
                            on uf.UserId equals v.UserId
                            into g
                            from v in g.DefaultIfEmpty()
                            where uf.FollowedUserId == userId
                            orderby uf.FollowTime descending
                            select new FollowUserItem
                            {
                                UserId = uf.UserId,
                                UserName = u.NickName,
                                UserAvatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = u.Description,
                                IsAnchor = a!=null,
                                Glamour = a ==null?0: a.Glamour,
                                NeteaseChatStatus = n == null ? 0: n.ChatStatus,
                                IsVip = v!=null,
                                FollowTime = uf.FollowTime,
                                UserPhotoItems = (
                                    from ph in context.TB_UserPhoto
                                    where ph.UserId == uf.UserId
                                    orderby ph.SortId
                                    select new UserPhotoItem
                                    {
                                        SortId = ph.SortId,
                                        ImageWebPath = ph.ImageWebPath,
                                        ThumbnailPath = ph.ThumbnailPath
                                    }
                                )
                            };
                return query.Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }
    }
}
