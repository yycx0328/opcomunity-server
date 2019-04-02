using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_TopicTagMap : EntityTypeConfiguration<TB_TopicTag>
    {
        public TB_TopicTagMap()
        {
            // Primary Key
            this.HasKey(t => new { t.TopicId, t.TagId });

            // Properties
            this.Property(t => t.TopicId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TagId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_TopicTag");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.TagId).HasColumnName("TagId");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
