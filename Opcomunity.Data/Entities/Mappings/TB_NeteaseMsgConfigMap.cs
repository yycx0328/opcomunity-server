using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseMsgConfigMap : EntityTypeConfiguration<TB_NeteaseMsgConfig>
    {
        public TB_NeteaseMsgConfigMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.TypeDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseMsgConfig");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.Seconds).HasColumnName("Seconds");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.TypeDescription).HasColumnName("TypeDescription");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.IsAvaiable).HasColumnName("IsAvaiable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
