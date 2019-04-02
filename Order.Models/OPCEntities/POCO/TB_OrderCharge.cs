using System;
using System.Collections.Generic;

namespace Opcomunity.Data.Entities
{
    public partial class TB_OrderCharge
    {
		public TB_OrderCharge ToPOCO(bool isPOCO = true){
			return new TB_OrderCharge{
				OrderId = this.OrderId,
				UserId = this.UserId,
				ApplicationId = this.ApplicationId,
				AppId = this.AppId,
				SellerId = this.SellerId,
				ChargeType = this.ChargeType,
				Subject = this.Subject,
				Body = this.Body,
				ChargeMoney = this.ChargeMoney,
				ExchargeRate = this.ExchargeRate,
				CoinCount = this.CoinCount,
				Status = this.Status,
				StatusDescription = this.StatusDescription,
				Ip = this.Ip,
				TakeOrderTime = this.TakeOrderTime,
				ChargeTime = this.ChargeTime,
				UpdateTime = this.UpdateTime,
			};
		}
    }
}
