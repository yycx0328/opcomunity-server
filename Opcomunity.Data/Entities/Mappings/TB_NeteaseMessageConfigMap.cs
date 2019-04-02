using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseMessageConfigMap : EntityTypeConfiguration<TB_NeteaseMessageConfig>
    {
        public TB_NeteaseMessageConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TypeDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(512);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseMessageConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.TypeDescription).HasColumnName("TypeDescription");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.IsAvaiable).HasColumnName("IsAvaiable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
