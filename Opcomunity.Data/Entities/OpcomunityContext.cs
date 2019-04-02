using Infrastructure;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Opcomunity.Data.Entities
{
    public class OpcomunityContext : DbContext
    {
        static OpcomunityContext()
        {
            Database.SetInitializer<OpcomunityContext>(null);
        }

        public OpcomunityContext() : base("name=OpcomunityContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var mappings = GetType().Assembly.GetInheritedTypes(typeof(EntityTypeConfiguration<>));
            foreach (var mapping in mappings)
            {
                dynamic instance = Activator.CreateInstance(mapping);
                modelBuilder.Configurations.Add(instance);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var item in error.ValidationErrors)
                    {
                        sb.AppendLine(item.PropertyName + ": " + item.ErrorMessage);
                    }
                }
                LogHelper.TryLog("SaveChanges.DbEntityValidation", ex.GetAllMessages() + sb);
                throw;
            }
        }

        public DbSet<TB_Anchor> TB_Anchor { get; set; }
        public DbSet<TB_AnchorCategory> TB_AnchorCategory { get; set; }
        public DbSet<TB_AnchorCategoryRelation> TB_AnchorCategoryRelation { get; set; }
        public DbSet<TB_AnchorIdentity> TB_AnchorIdentity { get; set; }
        public DbSet<TB_AppVersion> TB_AppVersion { get; set; }
        public DbSet<TB_AppVisitLog> TB_AppVisitLog { get; set; }
        public DbSet<TB_Banner> TB_Banner { get; set; }
        public DbSet<TB_CallAnchor> TB_CallAnchor { get; set; }
        public DbSet<TB_Channel> TB_Channel { get; set; }
        public DbSet<TB_Config> TB_Config { get; set; }
        public DbSet<TB_CashTransaction> TB_CashTransaction { get; set; }
        public DbSet<TB_CityConfig> TB_CityConfig { get; set; }
        public DbSet<TB_FeedBack> TB_FeedBack { get; set; }
        public DbSet<TB_Gift> TB_Gift { get; set; }
        public DbSet<TB_GiftTransaction> TB_GiftTransaction { get; set; }
        public DbSet<TB_JPushSms> TB_JPushSms { get; set; }
        public DbSet<TB_Message> TB_Message { get; set; }
        public DbSet<TB_NeteaseAccount> TB_NeteaseAccount { get; set; }
        public DbSet<TB_NeteaseCall> TB_NeteaseCall { get; set; }
        public DbSet<TB_NeteaseCallMember> TB_NeteaseCallMember { get; set; }
        public DbSet<TB_NeteaseText> TB_NeteaseText { get; set; }
        public DbSet<TB_NeteaseMessageConfig> TB_NeteaseMessageConfig { get; set; }
        public DbSet<TB_NeteaseMsgConfig> TB_NeteaseMsgConfig { get; set; }
        public DbSet<TB_NeteaseMessageSend> TB_NeteaseMessageSend { get; set; }
        public DbSet<TB_NeteaseMessageUser> TB_NeteaseMessageUser { get; set; }
        public DbSet<TB_NeteaseUserMessageRelation> TB_NeteaseUserMessageRelation { get; set; }
        public DbSet<TB_OrderAlipayCallbackResult> TB_OrderAlipayCallbackResult { get; set; }
        public DbSet<TB_OrderAlipayString> TB_OrderAlipayString { get; set; }
        public DbSet<TB_OrderCharge> TB_OrderCharge { get; set; }
        public DbSet<TB_OssObject> TB_OssObject { get; set; }
        public DbSet<TB_QiniuUploadToken> TB_QiniuUploadToken { get; set; }
        public DbSet<TB_SendBatchMessage> TB_SendBatchMessage { get; set; }
        public DbSet<TB_StatisticsChannel> TB_StatisticsChannel { get; set; }
        public DbSet<TB_Tag> TB_Tag { get; set; }
        public DbSet<TB_TicketConfig> TB_TicketConfig { get; set; }
        public DbSet<TB_TipOff> TB_TipOff { get; set; }
        public DbSet<TB_TipOffCategory> TB_TipOffCategory { get; set; }
        public DbSet<TB_Topic> TB_Topic { get; set; }
        public DbSet<TB_TopicCollect> TB_TopicCollect { get; set; }
        public DbSet<TB_TopicComment> TB_TopicComment { get; set; }
        public DbSet<TB_TopicTag> TB_TopicTag { get; set; }
        public DbSet<TB_UserAuth> TB_UserAuth { get; set; }
        public DbSet<TB_User> TB_User { get; set; }
        public DbSet<TB_UserCoin> TB_UserCoin { get; set; }
        public DbSet<TB_UserCoinJournal> TB_UserCoinJournal { get; set; }
        public DbSet<TB_UserIncomeJournal> TB_UserIncomeJournal { get; set; } 
        public DbSet<TB_UserFollow> TB_UserFollow { get; set; }
        public DbSet<TB_UserInvite> TB_UserInvite { get; set; }
        public DbSet<TB_UserTicket> TB_UserTicket { get; set; }
        public DbSet<TB_UserPhoto> TB_UserPhoto { get; set; }
        public DbSet<TB_UserTokenInfo> TB_UserTokenInfo { get; set; }
        public DbSet<TB_UserTopicPayment> TB_UserTopicPayment { get; set; }
        public DbSet<TB_UserVideo> TB_UserVideo { get; set; }
        public DbSet<TB_UserVideoPraise> TB_UserVideoPraise { get; set; }
        public DbSet<TB_UserVIP> TB_UserVIP { get; set; }
        public DbSet<TB_VIPConfig> TB_VIPConfig { get; set; }
    }
}