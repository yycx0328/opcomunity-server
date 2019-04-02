using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AnchorIdentityMap : EntityTypeConfiguration<TB_AnchorIdentity>
    {
        public TB_AnchorIdentityMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.IdentityPositive)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.IdentityOpposite)
                .HasMaxLength(256);

            this.Property(t => t.Remark)
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("TB_AnchorIdentity");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IdentityPositive).HasColumnName("IdentityPositive");
            this.Property(t => t.IdentityOpposite).HasColumnName("IdentityOpposite");
            this.Property(t => t.Remark).HasColumnName("Remark");
        }
    }
}
