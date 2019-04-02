using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseMessageUserMap : EntityTypeConfiguration<TB_NeteaseMessageUser>
    {
        public TB_NeteaseMessageUserMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseMessageUser");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
