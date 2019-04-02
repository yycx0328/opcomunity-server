using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserAuthMap : EntityTypeConfiguration<TB_UserAuth>
    {
        public TB_UserAuthMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IdentityType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Identifier)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Credential)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.FirstLoginApp)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_UserAuth");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.IdentityType).HasColumnName("IdentityType");
            this.Property(t => t.Identifier).HasColumnName("Identifier");
            this.Property(t => t.Credential).HasColumnName("Credential");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.FirstLoginApp).HasColumnName("FirstLoginApp");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.IsLegal).HasColumnName("IsLegal");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.LastLoginTime).HasColumnName("LastLoginTime");
        }
    }
}
