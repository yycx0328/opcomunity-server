using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserPhotoMap : EntityTypeConfiguration<TB_UserPhoto>
    {
        public TB_UserPhotoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.ImageWebPath)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.ThumbnailPath)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("TB_UserPhoto");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.ImageWebPath).HasColumnName("ImageWebPath");
            this.Property(t => t.ThumbnailPath).HasColumnName("ThumbnailPath");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
