using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AppVersionMap : EntityTypeConfiguration<TB_AppVersion>
    {
        public TB_AppVersionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Version)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Link)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.MinVersion)
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_AppVersion");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.Version).HasColumnName("Version");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.MinVersion).HasColumnName("MinVersion");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
