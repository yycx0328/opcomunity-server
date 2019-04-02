using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_OrderWechatPayClientPatameterMap : EntityTypeConfiguration<TB_OrderWechatPayClientPatameter>
    {
        public TB_OrderWechatPayClientPatameterMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.OrderId)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.AppId)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Noncestr)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.PatnerId)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.PrepayId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.TimeStamp)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.Sign)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("TB_OrderWechatPayClientPatameter");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.AppId).HasColumnName("AppId");
            this.Property(t => t.Noncestr).HasColumnName("Noncestr");
            this.Property(t => t.PatnerId).HasColumnName("PatnerId");
            this.Property(t => t.PrepayId).HasColumnName("PrepayId");
            this.Property(t => t.TimeStamp).HasColumnName("TimeStamp");
            this.Property(t => t.Sign).HasColumnName("Sign");
        }
    }
}
