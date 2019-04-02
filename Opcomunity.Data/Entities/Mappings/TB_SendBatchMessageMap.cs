using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_SendBatchMessageMap : EntityTypeConfiguration<TB_SendBatchMessage>
    {
        public TB_SendBatchMessageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Message)
                .IsRequired()
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("TB_SendBatchMessage");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Message).HasColumnName("Message");
            this.Property(t => t.SendCount).HasColumnName("SendCount");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
