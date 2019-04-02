using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_AdmUserChannelMap : EntityTypeConfiguration<TB_AdmUserChannel>
    {
        public TB_AdmUserChannelMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_AdmUserChannel");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.AdmUserId).HasColumnName("AdmUserId");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.Deduction).HasColumnName("Deduction");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
