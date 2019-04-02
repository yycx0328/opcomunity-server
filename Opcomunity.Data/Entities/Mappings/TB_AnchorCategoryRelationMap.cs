using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AnchorCategoryRelationMap : EntityTypeConfiguration<TB_AnchorCategoryRelation>
    {
        public TB_AnchorCategoryRelationMap()
        {
            // Primary Key
            this.HasKey(t => new { t.AnchorId, t.CategoryId });

            // Properties
            this.Property(t => t.AnchorId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.CategoryId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_AnchorCategoryRelation");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.CategoryId).HasColumnName("CategoryId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
