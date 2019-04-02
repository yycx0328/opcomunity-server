using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_GiftMap : EntityTypeConfiguration<TB_Gift>
    {
        public TB_GiftMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.Conver)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("TB_Gift");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Conver).HasColumnName("Conver");
            this.Property(t => t.OriginalPrice).HasColumnName("OriginalPrice");
            this.Property(t => t.IsDiscount).HasColumnName("IsDiscount");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.DiscountStart).HasColumnName("DiscountStart");
            this.Property(t => t.DiscountEnd).HasColumnName("DiscountEnd");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
