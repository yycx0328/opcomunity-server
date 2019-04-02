using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Services.Dtos;

namespace Opcomunity.Services.Implementations
{
    public class VideoService:ServiceBase, IVideoService
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
                var user = query.SingleOrDefault();
                return user != null;
            }
        }

        public void SaveUserVideo(TB_UserVideo model)
        {
            using (var context = base.NewContext())
            {
                context.TB_UserVideo.Add(model);
                context.SaveChanges();
            }
        }
        public List<VideoItem> GetVideoList(long userId, VideoListCategoryConfig category, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_UserVideo
                            join u in context.TB_User
                            on v.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p=>p.IsAuth && !p.IsBlack)
                            on v.UserId equals a.UserId
                            join n in context.TB_NeteaseAccount
                            on v.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            where v.IsAvailable
                            select new VideoItem
                            {
                                VideoId = v.Id,
                                AnchorId = v.UserId,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = v.Description,
                                IsAnchor = true,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                CallRatio = a.CallRatio,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                Link = v.Link,
                                ImgPath = v.ImgPath,
                                Praises = v.Praises,
                                IsPraise = (from p in context.TB_UserVideoPraise
                                            where p.UserId == userId && p.VideoId == v.Id
                                            select p).FirstOrDefault() != null,
                                Views = v.Views,
                                CreateTime = v.CreateTime
                            };
                if (category == VideoListCategoryConfig.Hottest)
                    return query.OrderByDescending(p => p.Praises).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                else if (category == VideoListCategoryConfig.Focus)
                {
                    var newQuery = from t in query
                                   join f in context.TB_UserFollow
                                   on t.AnchorId equals f.FollowedUserId
                                   where f.UserId == userId
                                   select t;
                    return newQuery.OrderByDescending(p => p.CreateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
                }
                return query.OrderByDescending(p => p.CreateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public PraiseVideoTips PraiseVideo(long userId, string token, long videoId)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.SingleOrDefault() == null)
                    return PraiseVideoTips.UserNotLoginErr;

                var qVideo = from v in context.TB_UserVideo
                             where v.IsAvailable && v.Id == videoId
                             select v;
                var video = qVideo.SingleOrDefault();
                if (video == null)
                    return PraiseVideoTips.VideoNotExistsErr;

                var query = from p in context.TB_UserVideoPraise
                            where p.UserId == userId && p.VideoId == videoId
                            select p;
                var videoPraise = query.SingleOrDefault();
                if(videoPraise == null)
                {
                    videoPraise = new TB_UserVideoPraise()
                    {
                        UserId = userId,
                        VideoId = videoId,
                        CreateTime = DateTime.Now
                    };
                    context.TB_UserVideoPraise.Add(videoPraise);
                    video.Praises += 1;
                    context.SaveChanges();
                    return PraiseVideoTips.PraiseSuccess;
                }
                else
                {
                    context.TB_UserVideoPraise.Remove(videoPraise);
                    if (video.Praises >= 1)
                        video.Praises -= 1;
                    context.SaveChanges();
                    return PraiseVideoTips.CancelPraiseSuccess;
                }
            }
        }

        public DeleteVideoTips DeleteVideo(long userId, string token, long videoId)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.SingleOrDefault() == null)
                    return DeleteVideoTips.UserNotLoginErr;

                var qVideo = from v in context.TB_UserVideo
                             where v.IsAvailable && v.Id == videoId
                             select v;
                var video = qVideo.SingleOrDefault();
                if (video == null)
                    return DeleteVideoTips.VideoNotExistsErr;
                if (video.UserId != userId)
                    return DeleteVideoTips.DeleteOthersVideoErr;

                context.TB_UserVideo.Remove(video);
                context.SaveChanges();
                return DeleteVideoTips.Success;
            }
        }

        public List<VideoItem> GetMyVideoList(long userId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_UserVideo
                            join u in context.TB_User
                            on v.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p=>p.IsAuth && !p.IsBlack)
                            on v.UserId equals a.UserId
                            join n in context.TB_NeteaseAccount
                            on v.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            where v.IsAvailable && v.UserId == userId
                            select new VideoItem
                            {
                                VideoId = v.Id,
                                AnchorId = v.UserId,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = v.Description,
                                IsAnchor = true,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                CallRatio = a.CallRatio,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                Link = v.Link,
                                ImgPath = v.ImgPath,
                                Praises = v.Praises,
                                IsPraise = (from p in context.TB_UserVideoPraise
                                            where p.UserId == userId && p.VideoId == v.Id
                                            select p).FirstOrDefault() != null,
                                Views = v.Views,
                                CreateTime = v.CreateTime
                            };
                return query.OrderByDescending(p => p.CreateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }

        public BasicTips ViewVideo(long userId, string token, long videoId)
        {
            using (var context = base.NewContext())
            {
                var qUser = from userInfo in context.TB_User
                            join tokenInfo in context.TB_UserTokenInfo
                            on userInfo.Id equals tokenInfo.UserId
                            where userInfo.Id == userId && tokenInfo.UserToken == token
                            select userInfo;
                if (qUser.SingleOrDefault() == null)
                    return BasicTips.UserNotLoginErr;

                var qVideo = from v in context.TB_UserVideo
                             where v.IsAvailable && v.Id == videoId
                             select v;
                var video = qVideo.SingleOrDefault();
                if (video != null)
                {
                    video.Views += 1;
                    context.SaveChanges();
                }
                return BasicTips.Success;
            }
        }

        public List<VideoItem> GetAnchorVideoList(long userId, long anchorId, int pageIndex, int pageSize)
        {
            using (var context = base.NewContext())
            {
                var query = from v in context.TB_UserVideo
                            join u in context.TB_User
                            on v.UserId equals u.Id
                            join a in context.TB_Anchor.Where(p => p.IsAuth && !p.IsBlack)
                            on v.UserId equals a.UserId
                            join n in context.TB_NeteaseAccount
                            on v.UserId equals n.UserId
                            into an
                            from n in an.DefaultIfEmpty()
                            where v.IsAvailable && v.UserId == anchorId
                            select new VideoItem
                            {
                                VideoId = v.Id,
                                AnchorId = v.UserId,
                                NickName = u.NickName,
                                Avatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                Description = v.Description,
                                IsAnchor = true,
                                AuthStatus = (int)AnchorAuthStatusConfig.Auth,
                                CallRatio = a.CallRatio,
                                NeteaseAccId = n == null ? "" : n.NeteaseAccId,
                                NeteaseChatStatus = n == null ? 0 : n.ChatStatus,
                                Link = v.Link,
                                ImgPath = v.ImgPath,
                                Praises = v.Praises,
                                IsPraise = (from p in context.TB_UserVideoPraise
                                            where p.UserId == userId && p.VideoId == v.Id
                                            select p).FirstOrDefault() != null,
                                Views = v.Views,
                                CreateTime = v.CreateTime
                            };
                return query.OrderByDescending(p => p.CreateTime).Take(pageSize * pageIndex).Skip(pageSize * (pageIndex - 1)).ToList();
            }
        }
    }
}
