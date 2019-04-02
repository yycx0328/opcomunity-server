using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsChannelMap : EntityTypeConfiguration<TB_StatisticsChannel>
    {
        public TB_StatisticsChannelMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.Channel });

            // Properties
            this.Property(t => t.Channel)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_StatisticsChannel");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.RegistCount).HasColumnName("RegistCount");
            this.Property(t => t.CoinCharge).HasColumnName("CoinCharge");
            this.Property(t => t.VipCharge).HasColumnName("VipCharge");
            this.Property(t => t.TicketCharge).HasColumnName("TicketCharge");
        }
    }
}
