using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseCallMemberMap : EntityTypeConfiguration<TB_NeteaseCallMember>
    {
        public TB_NeteaseCallMemberMap()
        {
            // Primary Key
            this.HasKey(t => new { t.ChannelId, t.AccId });

            // Properties
            this.Property(t => t.ChannelId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.AccId)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseCallMember");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.AccId).HasColumnName("AccId");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.IsCaller).HasColumnName("IsCaller");
        }
    }
}
