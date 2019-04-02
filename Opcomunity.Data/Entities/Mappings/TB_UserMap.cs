using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserMap : EntityTypeConfiguration<TB_User>
    {
        public TB_UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.NickName)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Avatar)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.ThumbnailAvatar)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.PhoneNo)
                .HasMaxLength(20);

            this.Property(t => t.WeChat)
                .HasMaxLength(32);

            this.Property(t => t.QQ)
                .HasMaxLength(20);

            this.Property(t => t.Constellation)
                .HasMaxLength(20);

            this.Property(t => t.AlipayAccount)
                .HasMaxLength(64);

            this.Property(t => t.Description)
                .HasMaxLength(256);

            // Table & Column Mappings
            this.ToTable("TB_User");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.NickName).HasColumnName("NickName");
            this.Property(t => t.Avatar).HasColumnName("Avatar");
            this.Property(t => t.ThumbnailAvatar).HasColumnName("ThumbnailAvatar");
            this.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            this.Property(t => t.WeChat).HasColumnName("WeChat");
            this.Property(t => t.QQ).HasColumnName("QQ");
            this.Property(t => t.Height).HasColumnName("Height");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.Constellation).HasColumnName("Constellation");
            this.Property(t => t.AlipayAccount).HasColumnName("AlipayAccount");
            this.Property(t => t.Description).HasColumnName("Description");
        }
    }
}
