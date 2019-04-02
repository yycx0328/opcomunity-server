using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserCoinMap : EntityTypeConfiguration<TB_UserCoin>
    {
        public TB_UserCoinMap()
        {
            // Primary Key
            this.HasKey(t => t.UserId);

            // Properties
            this.Property(t => t.UserId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            // Table & Column Mappings
            this.ToTable("TB_UserCoin");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CurrentCoin).HasColumnName("CurrentCoin");
            this.Property(t => t.CurrentIncome).HasColumnName("CurrentIncome");
        }
    }
}
