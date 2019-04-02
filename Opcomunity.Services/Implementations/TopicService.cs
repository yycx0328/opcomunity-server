using Opcomunity.Data.Entities;
using Opcomunity.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Implementations
{
    public class TopicService : ServiceBase, ITopicService
    {
        public void DeleteTopic(long topicId)
        {
            using (var context = base.NewContext())
            {
                var ossObject = context.TB_OssObject.Where(p => p.TopicId == topicId);
                context.TB_OssObject.RemoveRange(ossObject);
                var topic = context.TB_Topic.FirstOrDefault(p => p.Id == topicId);
                context.TB_Topic.Remove(topic);
                context.SaveChanges();
            }
        }

        public TB_Topic GetTopicById(long topicId)
        {
            using (var context = base.NewContext())
            {
                return context.TB_Topic.FirstOrDefault(p => p.Id == topicId);
            }
        }

        public long SaveTopic(TB_Topic model)
        {
            using (var context = base.NewContext())
            {
                context.TB_Topic.Add(model);
                context.SaveChanges();
                return model.Id;
            }
        }

        public bool SaveTopicOssObject(TB_OssObject model)
        {
            using (var context = base.NewContext())
            {
                context.TB_OssObject.Add(model);
                return context.SaveChanges()>0;
            }
        }
    }
}
