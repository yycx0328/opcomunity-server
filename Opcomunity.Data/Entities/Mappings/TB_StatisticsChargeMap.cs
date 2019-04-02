using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsChargeMap : EntityTypeConfiguration<TB_StatisticsCharge>
    {
        public TB_StatisticsChargeMap()
        {
            // Primary Key
            this.HasKey(t => t.Date);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_StatisticsCharge");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.TotalCharge).HasColumnName("TotalCharge");
            this.Property(t => t.ALCharge).HasColumnName("ALCharge");
            this.Property(t => t.WXCharge).HasColumnName("WXCharge");
        }
    }
}
