using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_OssObjectMap : EntityTypeConfiguration<TB_OssObject>
    {
        public TB_OssObjectMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Bucket)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OssKey)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.HashValue)
                .IsRequired()
                .HasMaxLength(128);

            this.Property(t => t.MimeType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ext)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_OssObject");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.Bucket).HasColumnName("Bucket");
            this.Property(t => t.OssKey).HasColumnName("OssKey");
            this.Property(t => t.HashValue).HasColumnName("HashValue");
            this.Property(t => t.FileSize).HasColumnName("FileSize");
            this.Property(t => t.MimeType).HasColumnName("MimeType");
            this.Property(t => t.Ext).HasColumnName("Ext");
            this.Property(t => t.SortId).HasColumnName("SortId");
            this.Property(t => t.IsLock).HasColumnName("IsLock");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
