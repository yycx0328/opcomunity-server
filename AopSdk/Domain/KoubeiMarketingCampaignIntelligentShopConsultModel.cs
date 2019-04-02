using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// KoubeiMarketingCampaignIntelligentShopConsultModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiMarketingCampaignIntelligentShopConsultModel : AopObject
    {
        /// <summary>
        /// 操作上下文
        /// </summary>
        [XmlElement("operator_context")]
        public PromoOperatorInfo OperatorContext { get; set; }

        /// <summary>
        /// 商户和支付宝交互时，用于代表支付宝分配给商户ID
        /// </summary>
        [XmlElement("partner_id")]
        public string PartnerId { get; set; }

        /// <summary>
        /// 智能营销方案的方案模板id
        /// </summary>
        [XmlElement("plan_id")]
        public string PlanId { get; set; }
    }
}
