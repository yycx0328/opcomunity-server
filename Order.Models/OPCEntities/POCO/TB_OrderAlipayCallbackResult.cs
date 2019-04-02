using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OrderAlipayCallbackResult
    {
		public TB_OrderAlipayCallbackResult ToPOCO(bool isPOCO = true){
			return new TB_OrderAlipayCallbackResult{
				Id = this.Id,
				NotifyId = this.NotifyId,
				AppId = this.AppId,
				TradeNo = this.TradeNo,
				OutTradeNo = this.OutTradeNo,
				NotifyTime = this.NotifyTime,
				NotifyType = this.NotifyType,
				Charset = this.Charset,
				Version = this.Version,
				SignType = this.SignType,
				Sign = this.Sign,
				BuyerId = this.BuyerId,
				BuyerLogonId = this.BuyerLogonId,
				SellerId = this.SellerId,
				SellerEmail = this.SellerEmail,
				TradeStatus = this.TradeStatus,
				TotalAmount = this.TotalAmount,
				ReceiptAmount = this.ReceiptAmount,
				BuyerPayAmount = this.BuyerPayAmount,
				Subject = this.Subject,
				Body = this.Body,
				GmtCreate = this.GmtCreate,
				GmtPayment = this.GmtPayment,
				Status = this.Status,
				StatusDescription = this.StatusDescription,
			};
		}
    }
}
