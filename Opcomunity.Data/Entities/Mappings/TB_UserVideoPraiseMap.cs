using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserVideoPraiseMap : EntityTypeConfiguration<TB_UserVideoPraise>
    {
        public TB_UserVideoPraiseMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.VideoId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.VideoId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_UserVideoPraise");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.VideoId).HasColumnName("VideoId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
