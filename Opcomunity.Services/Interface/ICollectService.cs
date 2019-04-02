using Opcomunity.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface ICollectService
    { 
        /// <summary>
        /// 获取收藏作品集合
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        List<TopicItem> GetCollectTopics(long userId);

        /// <summary>
        /// 收藏作品
        /// </summary>
        /// <param name="userId">收藏者Id</param>
        /// <param name="topicId">作品Id</param>
        /// <returns></returns>
        CollectTips CollectTopic(long userId, long topicId);

        /// <summary>
        /// 取消收藏作品
        /// </summary>
        /// <param name="userId">收藏者Id</param>
        /// <param name="topicId">作品Id</param>
        /// <returns></returns>
        CollectTips CancelCollect(long userId, long topicId);
    }
}
