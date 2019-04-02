using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_TopicCommentMap : EntityTypeConfiguration<TB_TopicComment>
    {
        public TB_TopicCommentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Comment)
                .IsRequired()
                .HasMaxLength(1024);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_TopicComment");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.TopicId).HasColumnName("TopicId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.Comment).HasColumnName("Comment");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.IsAvailable).HasColumnName("IsAvailable");
            this.Property(t => t.CommentTime).HasColumnName("CommentTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
