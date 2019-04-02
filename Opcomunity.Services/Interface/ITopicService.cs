using Opcomunity.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface ITopicService
    {
        /// <summary>
        /// 获取话题信息
        /// </summary>
        /// <param name="topicId"></param>
        /// <returns></returns>
        TB_Topic GetTopicById(long topicId);

        /// <summary>
        /// 保存作品信息
        /// </summary>
        /// <returns></returns>
        long SaveTopic(TB_Topic model);

        /// <summary>
        /// 保存话题文件对象
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveTopicOssObject(TB_OssObject model);

        /// <summary>
        /// 删除话题
        /// </summary>
        /// <param name="topicId"></param>
        void DeleteTopic(long topicId);
    }
}
