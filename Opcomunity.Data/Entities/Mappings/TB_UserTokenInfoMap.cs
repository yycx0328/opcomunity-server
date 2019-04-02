using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserTokenInfoMap : EntityTypeConfiguration<TB_UserTokenInfo>
    {
        public TB_UserTokenInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.UserToken)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_UserTokenInfo");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.UserToken).HasColumnName("UserToken");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.ExpireTime).HasColumnName("ExpireTime");
        }
    }
}
