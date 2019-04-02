using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseMessageSendMap : EntityTypeConfiguration<TB_NeteaseMessageSend>
    {
        public TB_NeteaseMessageSendMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.FromAccId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ToAccId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseMessageSend");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.FromUserId).HasColumnName("FromUserId");
            this.Property(t => t.FromAccId).HasColumnName("FromAccId");
            this.Property(t => t.ToUserId).HasColumnName("ToUserId");
            this.Property(t => t.ToAccId).HasColumnName("ToAccId");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.WaitingSendTime).HasColumnName("WaitingSendTime");
        }
    }
}
