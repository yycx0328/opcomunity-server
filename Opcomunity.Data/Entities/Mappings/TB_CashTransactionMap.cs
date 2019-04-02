using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_CashTransactionMap : EntityTypeConfiguration<TB_CashTransaction>
    {
        public TB_CashTransactionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Id)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.CashAccount)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.CashName)
                .IsRequired()
                .HasMaxLength(64);

            this.Property(t => t.StatusDescription)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_CashTransaction");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.CoinCount).HasColumnName("CoinCount");
            this.Property(t => t.CashMoney).HasColumnName("CashMoney");
            this.Property(t => t.CashRatio).HasColumnName("CashRatio");
            this.Property(t => t.CashAccount).HasColumnName("CashAccount");
            this.Property(t => t.CashName).HasColumnName("CashName");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.StatusDescription).HasColumnName("StatusDescription");
            this.Property(t => t.CashTime).HasColumnName("CashTime");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.UpdateDate).HasColumnName("UpdateDate");
        }
    }
}
