using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserVIPMap : EntityTypeConfiguration<TB_UserVIP>
    {
        public TB_UserVIPMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.VIPType)
                .IsRequired()
                .HasMaxLength(10);

            // Table & Column Mappings
            this.ToTable("TB_UserVIP");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.VIPType).HasColumnName("VIPType");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.EndTime).HasColumnName("EndTime");
            this.Property(t => t.CostMoney).HasColumnName("CostMoney");
            this.Property(t => t.TotalCoin).HasColumnName("TotalCoin");
            this.Property(t => t.TotalDay).HasColumnName("TotalDay");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
