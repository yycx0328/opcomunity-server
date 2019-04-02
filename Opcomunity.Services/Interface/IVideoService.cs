using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IVideoService
    {
        /// <summary>
        /// 验证是否为登录用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsLoginUser(long userId, string token);
        void SaveUserVideo(TB_UserVideo model);

        /// <summary>
        /// 发现主播列表
        /// </summary>
        /// <param name="category">话题类型（最新或最热）</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        List<VideoItem> GetVideoList(long userId, VideoListCategoryConfig category, int pageIndex, int pageSize);

        PraiseVideoTips PraiseVideo(long userId, string token, long videoId);

        DeleteVideoTips DeleteVideo(long userId, string token, long videoId);

        List<VideoItem> GetMyVideoList(long userId, int pageIndex, int pageSize);

        BasicTips ViewVideo(long userId, string token, long videoId);

        List<VideoItem> GetAnchorVideoList(long userId,long anchorId, int pageIndex, int pageSize);
    }
}
