using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseAccountMap : EntityTypeConfiguration<TB_NeteaseAccount>
    {
        public TB_NeteaseAccountMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.NeteaseAccId)
                .IsRequired()
                .HasMaxLength(32);

            this.Property(t => t.NeteaseToken)
                .IsRequired()
                .HasMaxLength(128);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseAccount");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.NeteaseAccId).HasColumnName("NeteaseAccId");
            this.Property(t => t.NeteaseToken).HasColumnName("NeteaseToken");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.LoginStatus).HasColumnName("LoginStatus");
            this.Property(t => t.ChatStatus).HasColumnName("ChatStatus");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
