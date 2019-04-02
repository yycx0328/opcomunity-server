using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserTicketMap : EntityTypeConfiguration<TB_UserTicket>
    {
        public TB_UserTicketMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Category)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_UserTicket");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.AnchorId).HasColumnName("AnchorId");
            this.Property(t => t.Category).HasColumnName("Category");
            this.Property(t => t.Cost).HasColumnName("Cost");
            this.Property(t => t.TotalTicket).HasColumnName("TotalTicket");
            this.Property(t => t.RemainderTicket).HasColumnName("RemainderTicket");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
            this.Property(t => t.UpdateTime).HasColumnName("UpdateTime");
        }
    }
}
