using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_FeedBackMap : EntityTypeConfiguration<TB_FeedBack>
    {
        public TB_FeedBackMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_FeedBack");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.FeedBackTime).HasColumnName("FeedBackTime");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.Ip).HasColumnName("Ip");
        }
    }
}
