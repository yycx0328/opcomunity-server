using System;
using System.Xml.Serialization;
using System.Collections.Generic;
using Aop.Api.Domain;

namespace Aop.Api.Response
{
    /// <summary>
    /// KoubeiMarketingCampaignIntelligentShopConsultResponse.
    /// </summary>
    public class KoubeiMarketingCampaignIntelligentShopConsultResponse : AopResponse
    {
        /// <summary>
        /// 智能营销方案符合标准的门店列表
        /// </summary>
        [XmlArray("shops")]
        [XmlArrayItem("intelligent_promo_shop_summary_info")]
        public List<IntelligentPromoShopSummaryInfo> Shops { get; set; }
    }
}
