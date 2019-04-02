using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsCashMap : EntityTypeConfiguration<TB_StatisticsCash>
    {
        public TB_StatisticsCashMap()
        {
            // Primary Key
            this.HasKey(t => t.Date);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_StatisticsCash");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.TotalCoinCount).HasColumnName("TotalCoinCount");
            this.Property(t => t.TotalCashMoney).HasColumnName("TotalCashMoney");
        }
    }
}
