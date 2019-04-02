using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_CityConfigMap : EntityTypeConfiguration<TB_CityConfig>
    {
        public TB_CityConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CityCode)
                .HasMaxLength(50);

            this.Property(t => t.CityShortName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CityFullName)
                .HasMaxLength(200);

            this.Property(t => t.CityLocation)
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("TB_CityConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CityCode).HasColumnName("CityCode");
            this.Property(t => t.CityShortName).HasColumnName("CityShortName");
            this.Property(t => t.CityFullName).HasColumnName("CityFullName");
            this.Property(t => t.CityLocation).HasColumnName("CityLocation");
        }
    }
}
