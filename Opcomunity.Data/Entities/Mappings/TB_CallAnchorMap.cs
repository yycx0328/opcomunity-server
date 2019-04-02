using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_CallAnchorMap : EntityTypeConfiguration<TB_CallAnchor>
    {
        public TB_CallAnchorMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("TB_CallAnchor");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CallId).HasColumnName("CallId");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.Sender).HasColumnName("Sender");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.CallRatio).HasColumnName("CallRatio");
            this.Property(t => t.TotalFee).HasColumnName("TotalFee");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
