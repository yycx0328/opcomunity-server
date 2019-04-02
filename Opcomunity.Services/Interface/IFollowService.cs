using Opcomunity.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IFollowService
    {
        /// <summary>
        /// 校验是否为登陆用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        bool IsLoginUser(long userId, string token);
        /// <summary>
        /// 获取关注用户集合
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        List<FollowUserItem> GetFollowUsers(long userId, int pageIndex, int pageSize);

        /// <summary>
        /// 获取关注我的用户集合
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <returns></returns>
        List<FollowUserItem> GetFollowMineUsers(long userId, int pageIndex, int pageSize);

        /// <summary>
        /// 关注用户
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="followedUserId">被关注用户的Id</param>
        /// <returns></returns>
        FollowTips FollowUser(long userId, long followedUserId);

        /// <summary>
        /// 取消关注
        /// </summary>
        /// <param name="userId">当前用户Id</param>
        /// <param name="followedUserId">被关注用户的Id</param>
        /// <returns></returns>
        FollowTips CancelFollow(long userId, long followedUserId);
    }
}
