using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserFollowMap : EntityTypeConfiguration<TB_UserFollow>
    {
        public TB_UserFollowMap()
        {
            // Primary Key
            this.HasKey(t => new { t.UserId, t.FollowedUserId });

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.FollowedUserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_UserFollow");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.FollowedUserId).HasColumnName("FollowedUserId");
            this.Property(t => t.FollowTime).HasColumnName("FollowTime");
        }
    }
}
