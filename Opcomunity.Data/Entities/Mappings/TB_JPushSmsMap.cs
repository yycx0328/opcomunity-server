using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_JPushSmsMap : EntityTypeConfiguration<TB_JPushSms>
    {
        public TB_JPushSmsMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.MessageId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Code)
                .HasMaxLength(10);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_JPushSms");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            this.Property(t => t.TemplateId).HasColumnName("TemplateId");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
