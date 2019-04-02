using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsNeteaseCallMap : EntityTypeConfiguration<TB_StatisticsNeteaseCall>
    {
        public TB_StatisticsNeteaseCallMap()
        {
            // Primary Key
            this.HasKey(t => t.Date);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_StatisticsNeteaseCall");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.TotalDuration).HasColumnName("TotalDuration");
            this.Property(t => t.TotalFee).HasColumnName("TotalFee");
            this.Property(t => t.TotalActualFee).HasColumnName("TotalActualFee");
        }
    }
}
