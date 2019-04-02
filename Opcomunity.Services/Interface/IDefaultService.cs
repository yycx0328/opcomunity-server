using Opcomunity.Data.Entities;
using Opcomunity.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opcomunity.Services.Interface
{
    public interface IDefaultService
    {
        void DataInitialize();

        int SaveFeedBack(long userId, string token, string description);
        List<TipOffCategoryItem> GetTipOffCategory();
        int SaveTipOff(long userId, string token, long anchorId, int categoryId, string description);
        List<GiftItem> GetGifts();
        List<MessageItem> GetMessage(long userId, string token, int pageIndex, int pageSize);
        List<InviteRewardItem> GetInviteRewardRankList(int pageIndex, int pageSize, out BasicTips tips);
        TB_AppVersion CheckAppUpdate(int channel);
        List<TB_StatisticsChannel> GetChannelStatistics(int channel, DateTime startDate, DateTime endDate);
    }
}
