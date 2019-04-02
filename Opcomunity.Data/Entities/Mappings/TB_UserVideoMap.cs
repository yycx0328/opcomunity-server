using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserVideoMap : EntityTypeConfiguration<TB_UserVideo>
    {
        public TB_UserVideoMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.Link)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(t => t.Extention)
                .IsRequired()
                .HasMaxLength(8);

            this.Property(t => t.ImgPath)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TB_UserVideo");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Link).HasColumnName("Link");
            this.Property(t => t.Extention).HasColumnName("Extention");
            this.Property(t => t.ImgPath).HasColumnName("ImgPath");
            this.Property(t => t.Views).HasColumnName("Views");
            this.Property(t => t.Praises).HasColumnName("Praises");
            this.Property(t => t.IsFree).HasColumnName("IsFree");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
