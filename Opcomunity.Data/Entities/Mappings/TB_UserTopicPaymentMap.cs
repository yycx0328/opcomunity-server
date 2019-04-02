using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserTopicPaymentMap : EntityTypeConfiguration<TB_UserTopicPayment>
    {
        public TB_UserTopicPaymentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_UserTopicPayment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.OriginalPrice).HasColumnName("OriginalPrice");
            this.Property(t => t.ActualPrice).HasColumnName("ActualPrice");
            this.Property(t => t.IsDiscount).HasColumnName("IsDiscount");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
