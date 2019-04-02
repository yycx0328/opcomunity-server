using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_ConfigMap : EntityTypeConfiguration<TB_Config>
    {
        public TB_ConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.KeyId);

            // Properties
            this.Property(t => t.KeyId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Value)
                .IsRequired()
                .HasMaxLength(2048);

            // Table & Column Mappings
            this.ToTable("TB_Config");
            this.Property(t => t.KeyId).HasColumnName("KeyId");
            this.Property(t => t.Value).HasColumnName("Value");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
