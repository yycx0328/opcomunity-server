using Opcomunity.Services.Dtos;
using System.Collections.Generic;
using Utility.Common;

namespace Opcomunity.Services.Interface
{
    public interface ICoinService
    {
        bool IsLoginUser(long userId, string token);
        void ModifyNetease();
        /// <summary>
        /// 费用单出流水
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="outCount"></param>
        /// <param name="outJournal"></param>
        /// <returns></returns>
        CashOutTips CoinSingleOut(long userId, int outCount, CoinJournalConfig outJournal, out long currentCoinCount);
        
        /// <summary>
        /// 金币交易日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CoinJournalItem> GetCoinTransactionRecord(long userId,int pageIndex, int pageSize);

        /// <summary>
        /// 金币交易日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<CoinJournalItem> GetCoinIncomeRecord(long userId, int pageIndex, int pageSize);
    }
}
