using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsCallTimesMap : EntityTypeConfiguration<TB_StatisticsCallTimes>
    {
        public TB_StatisticsCallTimesMap()
        {
            // Primary Key
            this.HasKey(t => t.Date);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_StatisticsCallTimes");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.TotalTimes).HasColumnName("TotalTimes");
            this.Property(t => t.TotalSuccessTimes).HasColumnName("TotalSuccessTimes");
            this.Property(t => t.TotalFaildTimes).HasColumnName("TotalFaildTimes");
        }
    }
}
