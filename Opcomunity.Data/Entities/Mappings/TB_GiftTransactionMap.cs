using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_GiftTransactionMap : EntityTypeConfiguration<TB_GiftTransaction>
    {
        public TB_GiftTransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_GiftTransaction");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.GiftId).HasColumnName("GiftId");
            this.Property(t => t.OriginalPrice).HasColumnName("OriginalPrice");
            this.Property(t => t.CostPrice).HasColumnName("CostPrice");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
