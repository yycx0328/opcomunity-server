namespace Opcomunity.Services.Interface
{
    public interface INeteaseService
    {
        /// <summary>
        /// 记录视频通话
        /// </summary>
        /// <param name="md5"></param>
        /// <param name="curTime"></param>
        /// <param name="checkSum"></param>
        /// <param name="requestBody"></param>
        /// <param name="channelId"></param>
        /// <param name="userId"></param>
        /// <param name="anchorId"></param>
        /// <param name="totalFee"></param>
        /// <returns></returns>
        NeteaseCallNotifyTips SaveLiveCallRecord(string md5, string curTime, string checkSum, string requestBody);

        /// <summary>
        /// 回滚通话记录
        /// </summary>
        /// <param name="channelId"></param>
        void RollbackLiveCallRecord(long channelId, int status, string statusDescription);

        /// <summary>
        /// 更新主播魅力值
        /// </summary>
        /// <param name="anchorId"></param>
        /// <param name="glamour"></param>
        /// <returns></returns>
        void UpdateAnchorGlamour(long anchorId, long glamour);
    }
}
