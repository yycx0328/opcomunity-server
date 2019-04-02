using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayUserAuthZhimaperInternalApplyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayUserAuthZhimaperInternalApplyModel : AopObject
    {
        /// <summary>
        /// 证件号码
        /// </summary>
        [XmlElement("cert_no")]
        public string CertNo { get; set; }

        /// <summary>
        /// IDENTITY_CARD :身份证
        /// </summary>
        [XmlElement("cert_type")]
        public string CertType { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [XmlElement("mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// 个人用户姓名
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
