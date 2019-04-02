using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AppVisitLogMap : EntityTypeConfiguration<TB_AppVisitLog>
    {
        public TB_AppVisitLogMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Version)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.OS)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TB_AppVisitLog");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.OS).HasColumnName("OS");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
