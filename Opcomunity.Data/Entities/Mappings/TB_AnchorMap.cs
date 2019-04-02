using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AnchorMap : EntityTypeConfiguration<TB_Anchor>
    {
        public TB_AnchorMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Description)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_Anchor");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.Glamour).HasColumnName("Glamour");
            this.Property(t => t.CashRatio).HasColumnName("CashRatio");
            this.Property(t => t.CallRatio).HasColumnName("CallRatio");
            this.Property(t => t.ApplyTime).HasColumnName("ApplyTime");
            this.Property(t => t.IsAuth).HasColumnName("IsAuth");
            this.Property(t => t.AuthTime).HasColumnName("AuthTime");
            this.Property(t => t.IsBlack).HasColumnName("IsBlack");
        }
    }
}
