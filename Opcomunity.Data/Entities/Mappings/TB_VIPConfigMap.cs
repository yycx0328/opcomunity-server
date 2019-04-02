using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_VIPConfigMap : EntityTypeConfiguration<TB_VIPConfig>
    {
        public TB_VIPConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Key);

            // Properties
            this.Property(t => t.Key)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_VIPConfig");
            this.Property(t => t.Key).HasColumnName("Key");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OriginalPrice).HasColumnName("OriginalPrice");
            this.Property(t => t.DiscountPrice).HasColumnName("DiscountPrice");
            this.Property(t => t.DonateCoin).HasColumnName("DonateCoin");
            this.Property(t => t.IsRecommand).HasColumnName("IsRecommand");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
