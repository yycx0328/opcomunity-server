using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Passport.Entities
{
    public class TB_ApplicationInfoMap : EntityTypeConfiguration<TB_ApplicationInfo>
    {
        public TB_ApplicationInfoMap()
        {
            // Primary Key
            this.HasKey(t => t.ApplicationId);

            // Properties
            this.Property(t => t.ApplicationId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ApplicationName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ApplicationKey)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.SecurityIps)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Status)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            // Table & Column Mappings
            this.ToTable("TB_ApplicationInfo");
            this.Property(t => t.ApplicationId).HasColumnName("ApplicationId");
            this.Property(t => t.ApplicationName).HasColumnName("ApplicationName");
            this.Property(t => t.ApplicationKey).HasColumnName("ApplicationKey");
            this.Property(t => t.SecurityIps).HasColumnName("SecurityIps");
            this.Property(t => t.Status).HasColumnName("Status");
        }
    }
}
