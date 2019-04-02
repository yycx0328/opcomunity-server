using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_MessageMap : EntityTypeConfiguration<TB_Message>
    {
        public TB_MessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.CategoryDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Title)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Content)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.Parameters)
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("TB_Message");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.CategoryDescription).HasColumnName("CategoryDescription");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.Content).HasColumnName("Content");
            this.Property(t => t.Parameters).HasColumnName("Parameters");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsRead).HasColumnName("IsRead");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
