using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_BannerMap : EntityTypeConfiguration<TB_Banner>
    {
        public TB_BannerMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Image)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Link)
                .HasMaxLength(256);

            this.Property(t => t.Description)
                .HasMaxLength(1024);

            this.Property(t => t.Parameters)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_Banner");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
