using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_TopicMap : EntityTypeConfiguration<TB_Topic>
    {
        public TB_TopicMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_Topic");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Price).HasColumnName("Price");
            this.Property(t => t.Discount).HasColumnName("Discount");
            this.Property(t => t.Views).HasColumnName("Views");
            this.Property(t => t.Collects).HasColumnName("Collects");
            this.Property(t => t.Comments).HasColumnName("Comments");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
