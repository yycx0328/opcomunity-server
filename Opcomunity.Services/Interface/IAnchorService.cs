using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using System.Collections.Generic;
using Utility.Common;

namespace Opcomunity.Services.Interface
{
    public interface IAnchorService
    {
        /// <summary>
        /// 推荐主播列表
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        List<AnchorItem> GetRecormmendAnchorList(int pageIndex, int pageSize);

        AnchorItem GetRandomAnchor();

        SendBatchMessageTips SendBatchMsg(long userId, string token, string message);

        List<TB_AnchorCategory> GetAnchorCategoryList();

        /// <summary>
        /// 发现主播列表
        /// </summary>
        /// <param name="category">话题类型（最新或最热）</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        List<AnchorItem> GetDiscoverAnchorList(int category, int pageIndex, int pageSize);

        /// <summary>
        /// 获取主播详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <param name="anchorId"></param>
        /// <returns></returns>
        AnchorDetailInfoItem GetAnchorDetailInfo(long userId, string token, long anchorId);

        /// <summary>
        /// 轮播图列表
        /// </summary>
        /// <param name="top"></param>
        /// <returns></returns>
        List<BannerItem> GetBannerList(int top);

        /// <summary>
        /// 获取登陆用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        TB_User GetLoginUserInfo(long userId, string token);

        /// <summary>
        /// 验证主播是否存在
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsAnchorExist(long userId, string token);

        /// <summary>
        /// 保存主播申请
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        AplayTobeAnchorTips SaveAnchor(TB_Anchor model);

        /// <summary>
        /// 保存用户提交的身份认证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        bool SaveAnchorIdentity(TB_AnchorIdentity model);
        
        /// <summary>
        /// 回滚提现记录
        /// </summary>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        List<DevoteRankItem> GetDevoteRank(long anchorId);

        /// <summary>
        /// 主播设置呼叫费用
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        BasicTips SetCallRate(long userId, string token, int callRatio);

    }
}
