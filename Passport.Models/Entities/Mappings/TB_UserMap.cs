using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Passport.Entities
{
    public class TB_UserMap : EntityTypeConfiguration<TB_User>
    {
        public TB_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            this.Property(t => t.RegistApplication)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.RegistApplication).HasColumnName("RegistApplication");
            this.Property(t => t.IsLegal).HasColumnName("IsLegal");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
