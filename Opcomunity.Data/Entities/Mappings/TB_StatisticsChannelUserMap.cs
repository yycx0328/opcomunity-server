using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_StatisticsChannelUserMap : EntityTypeConfiguration<TB_StatisticsChannelUser>
    {
        public TB_StatisticsChannelUserMap()
        {
            // Primary Key
            this.HasKey(t => new { t.Date, t.Channel });

            // Properties
            this.Property(t => t.Channel)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_StatisticsChannelUser");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Channel).HasColumnName("Channel");
            this.Property(t => t.RegistCount).HasColumnName("RegistCount");
            this.Property(t => t.ChargeMoney).HasColumnName("ChargeMoney");
        }
    }
}
