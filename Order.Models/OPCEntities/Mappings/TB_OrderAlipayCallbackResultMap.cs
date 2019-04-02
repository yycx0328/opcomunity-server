using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_OrderAlipayCallbackResultMap : EntityTypeConfiguration<TB_OrderAlipayCallbackResult>
    {
        public TB_OrderAlipayCallbackResultMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NotifyId)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.AppId)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.TradeNo)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.OutTradeNo)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.NotifyType)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Charset)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Version)
                .IsRequired()
                .HasMaxLength(6);

            this.Property(t => t.SignType)
                .IsRequired()
                .HasMaxLength(10);

            this.Property(t => t.Sign)
                .IsRequired()
                .HasMaxLength(2048);

            this.Property(t => t.BuyerId)
                .HasMaxLength(16);

            this.Property(t => t.BuyerLogonId)
                .HasMaxLength(100);

            this.Property(t => t.SellerId)
                .HasMaxLength(30);

            this.Property(t => t.SellerEmail)
                .HasMaxLength(100);

            this.Property(t => t.TradeStatus)
                .HasMaxLength(32);

            this.Property(t => t.Subject)
                .HasMaxLength(256);

            this.Property(t => t.Body)
                .HasMaxLength(400);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_OrderAlipayCallbackResult");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NotifyId).HasColumnName("NotifyId");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.TradeNo).HasColumnName("TradeNo");
            this.Property(t => t.OutTradeNo).HasColumnName("OutTradeNo");
            this.Property(t => t.NotifyTime).HasColumnName("NotifyTime");
            this.Property(t => t.NotifyType).HasColumnName("NotifyType");
            this.Property(t => t.Charset).HasColumnName("Charset");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.SignType).HasColumnName("SignType");
            this.Property(t => t.Sign).HasColumnName("Sign");
            this.Property(t => t.BuyerId).HasColumnName("BuyerId");
            this.Property(t => t.BuyerLogonId).HasColumnName("BuyerLogonId");
            this.Property(t => t.SellerId).HasColumnName("SellerId");
            this.Property(t => t.SellerEmail).HasColumnName("SellerEmail");
            this.Property(t => t.TradeStatus).HasColumnName("TradeStatus");
            this.Property(t => t.TotalAmount).HasColumnName("TotalAmount");
            this.Property(t => t.ReceiptAmount).HasColumnName("ReceiptAmount");
            this.Property(t => t.BuyerPayAmount).HasColumnName("BuyerPayAmount");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.GmtCreate).HasColumnName("GmtCreate");
            this.Property(t => t.GmtPayment).HasColumnName("GmtPayment");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
        }
    }
}
