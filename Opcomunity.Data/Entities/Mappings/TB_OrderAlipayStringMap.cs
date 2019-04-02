using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_OrderAlipayStringMap : EntityTypeConfiguration<TB_OrderAlipayString>
    {
        public TB_OrderAlipayStringMap()
        {
            // Primary Key
            this.HasKey(t => t.OrderId);

            // Properties
            this.Property(t => t.OrderId)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.OrderString)
                .IsRequired();

            // Table & Column Mappings
            this.ToTable("TB_OrderAlipayString");
            this.Property(t => t.OrderId).HasColumnName("OrderId");
            this.Property(t => t.OrderString).HasColumnName("OrderString");
        }
    }
}
