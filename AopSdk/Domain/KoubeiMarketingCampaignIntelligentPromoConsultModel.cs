using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Domain
{
    /// <summary>
    /// KoubeiMarketingCampaignIntelligentPromoConsultModel Data Structure.
    /// </summary>
    [Serializable]
    public class KoubeiMarketingCampaignIntelligentPromoConsultModel : AopObject
    {
        /// <summary>
        /// 操作人信息
        /// </summary>
        [XmlElement("operator_context")]
        public PromoOperatorInfo OperatorContext { get; set; }

        /// <summary>
        /// 商户和支付宝交互时，用于代表支付宝分配给商户ID
        /// </summary>
        [XmlElement("partner_id")]
        public string PartnerId { get; set; }

        /// <summary>
        /// 当全场普通和单品普通方案时必传，体验方案不用传
        /// </summary>
        [XmlArray("shop_ids")]
        [XmlArrayItem("string")]
        public List<string> ShopIds { get; set; }

        /// <summary>
        /// 营销模板的编号，GENERAL_EXPERIENCE：全场体验；GENERAL_NORMAL：全场普通；ITEM_EXPERIENCE：单品体验；ITEM_NORMAL：单品普通
        /// </summary>
        [XmlElement("template_code")]
        public string TemplateCode { get; set; }
    }
}
