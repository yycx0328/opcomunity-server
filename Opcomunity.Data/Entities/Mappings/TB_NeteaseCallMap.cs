using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseCallMap : EntityTypeConfiguration<TB_NeteaseCall>
    {
        public TB_NeteaseCallMap()
        {
            // Primary Key
            this.HasKey(t => t.ChannelId);

            // Properties
            this.Property(t => t.ChannelId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(t => t.Ext)
                .IsRequired()
                .HasMaxLength(512);

            this.Property(t => t.Type)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CallStatus)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseCall");
            this.Property(t => t.ChannelId).HasColumnName("ChannelId");
            this.Property(t => t.CallerId).HasColumnName("CallerId");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.TotalDuration).HasColumnName("TotalDuration");
            this.Property(t => t.Duration).HasColumnName("Duration");
            this.Property(t => t.CallRatio).HasColumnName("CallRatio");
            this.Property(t => t.TotalFee).HasColumnName("TotalFee");
            this.Property(t => t.ActualTransferFee).HasColumnName("ActualTransferFee");
            this.Property(t => t.Ext).HasColumnName("Ext");
            this.Property(t => t.CallTime).HasColumnName("CallTime");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.Live).HasColumnName("Live");
            this.Property(t => t.CallStatus).HasColumnName("CallStatus");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
