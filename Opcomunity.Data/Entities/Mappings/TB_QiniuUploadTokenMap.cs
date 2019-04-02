using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_QiniuUploadTokenMap : EntityTypeConfiguration<TB_QiniuUploadToken>
    {
        public TB_QiniuUploadTokenMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Token)
                .IsRequired()
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("TB_QiniuUploadToken");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Token).HasColumnName("Token");
            this.Property(t => t.ExpiresTime).HasColumnName("ExpiresTime");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
