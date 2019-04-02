using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserIncomeJournalMap : EntityTypeConfiguration<TB_UserIncomeJournal>
    {
        public TB_UserIncomeJournalMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.IOStatus)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(1);

            this.Property(t => t.JournalDesc)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            this.ToTable("TB_UserIncomeJournal");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.OriginUserId).HasColumnName("OriginUserId");
            this.Property(t => t.IncomeCount).HasColumnName("IncomeCount");
            this.Property(t => t.CurrentCount).HasColumnName("CurrentCount");
            this.Property(t => t.IOStatus).HasColumnName("IOStatus");
            this.Property(t => t.JournalType).HasColumnName("JournalType");
            this.Property(t => t.JournalDesc).HasColumnName("JournalDesc");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.CreateTime).HasColumnName("CreateTime");
        }
    }
}
