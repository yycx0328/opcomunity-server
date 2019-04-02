using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AnchorCategoryMap : EntityTypeConfiguration<TB_AnchorCategory>
    {
        public TB_AnchorCategoryMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_AnchorCategory");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
