using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseTextMap : EntityTypeConfiguration<TB_NeteaseText>
    {
        public TB_NeteaseTextMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MsgType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FromNick)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.MsgIdServer)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FromAccount)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.ToAccount)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FromClientType)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.FromDeviceId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Body)
                .IsRequired()
                .HasMaxLength(256);

            this.Property(t => t.ConvType)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.MsgIdClient)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_NeteaseText");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.EventType).HasColumnName("EventType");
            this.Property(t => t.MsgType).HasColumnName("MsgType");
            this.Property(t => t.FromNick).HasColumnName("FromNick");
            this.Property(t => t.MsgIdServer).HasColumnName("MsgIdServer");
            this.Property(t => t.FromAccount).HasColumnName("FromAccount");
            this.Property(t => t.FromUserId).HasColumnName("FromUserId");
            this.Property(t => t.ToAccount).HasColumnName("ToAccount");
            this.Property(t => t.ToUserId).HasColumnName("ToUserId");
            this.Property(t => t.FromClientType).HasColumnName("FromClientType");
            this.Property(t => t.FromDeviceId).HasColumnName("FromDeviceId");
            this.Property(t => t.Body).HasColumnName("Body");
            this.Property(t => t.ConvType).HasColumnName("ConvType");
            this.Property(t => t.MsgIdClient).HasColumnName("MsgIdClient");
            this.Property(t => t.ResendFlag).HasColumnName("ResendFlag");
            this.Property(t => t.MsgTimestamp).HasColumnName("MsgTimestamp");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
