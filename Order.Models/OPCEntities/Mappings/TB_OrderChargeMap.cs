using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_OrderChargeMap : EntityTypeConfiguration<TB_OrderCharge>
    {
        public TB_OrderChargeMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderId);

            // Properties
            this.Property(t => t.OrderId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ApplicationId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.AppId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SellerId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ChargeType)
                .IsRequired()
                .HasMaxLength(4);

            this.Property(t => t.Subject)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_OrderCharge");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.SellerId).HasColumnName("SellerId");
            this.Property(t => t.ChargeType).HasColumnName("ChargeType");
            this.Property(t => t.Subject).HasColumnName("Subject");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.ChargeMoney).HasColumnName("ChargeMoney");
            this.Property(t => t.ExchargeRate).HasColumnName("ExchargeRate");
            this.Property(t => t.CoinCount).HasColumnName("CoinCount");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.TakeOrderTime).HasColumnName("TakeOrderTime");
            this.Property(t => t.ChargeTime).HasColumnName("ChargeTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
