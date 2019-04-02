using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opcomunity.Services.Dtos;
using Opcomunity.Data.Entities;

namespace Opcomunity.Services.Implementations
{
    public class CollectService : ServiceBase, ICollectService
    {
        public CollectTips CancelCollect(long userId, long topicId)
        {
            using (var context = base.NewContext())
            {
                var query = from tc in context.TB_TopicCollect
                            where tc.UserId == userId && tc.TopicId == topicId
                            select tc;
                if (query.Count() == 0)
                    return CollectTips.UnCollectErr;

                context.TB_TopicCollect.RemoveRange(query);
                int result = context.SaveChanges();
                if (result > 0)
                    return CollectTips.Success;
                else
                    return CollectTips.CancelCollectFaild;
            }
        }

        public CollectTips CollectTopic(long userId, long topicId)
        {
            using (var context = base.NewContext())
            {
                var query = from tc in context.TB_TopicCollect
                            where tc.UserId == userId && tc.TopicId == topicId
                            select tc;
                if (query.Count() >= 1)
                    return CollectTips.AlreadyCollectErr;

                var queryTopic = from topic in context.TB_Topic where topic.Id == topicId select topic;
                if (queryTopic.Count() == 0)
                    return CollectTips.TopicNotExistErr;

                TB_TopicCollect collect = new TB_TopicCollect()
                {
                    UserId = userId,
                    TopicId = topicId,
                    CollectTime = DateTime.Now
                };
                context.TB_TopicCollect.Add(collect);
                int result = context.SaveChanges();
                if (result == 1)
                    return CollectTips.Success;
                else
                    return CollectTips.CollectFaild;
            }
        }

        public List<TopicItem> GetCollectTopics(long userId)
        {
            using (var context = base.NewContext())
            {
                var query = from collect in context.TB_TopicCollect
                            join topic in context.TB_Topic
                            on collect.TopicId equals topic.Id
                            join u in context.TB_User
                            on topic.UserId equals u.Id
                            join utp in context.TB_UserTopicPayment.DefaultIfEmpty()
                            on new { TopicId = topic.Id, UserId = u.Id } equals new { TopicId = utp.TopicId, UserId = utp.UserId }
                            where collect.UserId == userId
                            orderby collect.CollectTime descending
                            select new TopicItem
                            {
                                UserId = topic.UserId,
                                UserName = u.NickName,
                                UserAvatar = u.Avatar,
                                ThumbnailAvatar = u.ThumbnailAvatar,
                                TopicId = topic.Id,
                                TopicPrice = topic.Price,
                                TopicDescription = topic.Description,
                                IsLock = utp == null,
                                ViewCount = topic.Views,
                                CollectCount = topic.Collects,
                                CommentCount = topic.Comments,
                                TopicDateTime = collect.CollectTime,
                                TopicItems = (
                                    from oss in context.TB_OssObject 
                                    where oss.Id == collect.TopicId
                                    orderby oss.SortId
                                    select new TopicObjectItem
                                    {
                                        TopicId = oss.TopicId,
                                        OssKey = oss.OssKey,
                                        Bucket = oss.Bucket,
                                        FileSize = oss.FileSize,
                                        HashValue = oss.HashValue,
                                        MimeType = oss.MimeType,
                                        Ext = oss.Ext,
                                        SortId = oss.SortId,
                                        IsLock = oss.IsLock
                                    }
                                ),
                                TagItems = (
                                    from ttag in context.TB_TopicTag
                                    join tag in context.TB_Tag
                                    on ttag.TagId equals tag.Id
                                    where ttag.TopicId == collect.TopicId 
                                    select new TagItem
                                    {
                                        TagId = ttag.TagId,
                                        TagName = tag.Name,
                                        SortId = ttag.SortId
                                    }
                                )
                            };
                return query.ToList();
            }
        }
    }
}
