using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_TopicCollectMap : EntityTypeConfiguration<TB_TopicCollect>
    {
        public TB_TopicCollectMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.TopicId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.TopicId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_TopicCollect");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.CollectTime).HasColumnName("CollectTime");
        }
    }
}
