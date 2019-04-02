using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsCoinMap : EntityTypeConfiguration<TB_StatisticsCoin>
    {
        public TB_StatisticsCoinMap()
        {
            // Primary Key
            this.HasKey(t => t.Date);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_StatisticsCoin");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.TotalRegistGive).HasColumnName("TotalRegistGive");
            this.Property(t => t.TotalCharge).HasColumnName("TotalCharge");
            this.Property(t => t.TotalActiveGive).HasColumnName("TotalActiveGive");
            this.Property(t => t.TotalSendGift).HasColumnName("TotalSendGift");
            this.Property(t => t.TotalLiveCall).HasColumnName("TotalLiveCall");
            this.Property(t => t.TotalCash).HasColumnName("TotalCash");
            this.Property(t => t.TotalCashMoney).HasColumnName("TotalCashMoney");
            this.Property(t => t.TotalInvite).HasColumnName("TotalInvite");
            this.Property(t => t.TotalIn).HasColumnName("TotalIn");
            this.Property(t => t.TotalOut).HasColumnName("TotalOut");
            this.Property(t => t.TotalRemaind).HasColumnName("TotalRemaind");
        }
    }
}
