using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_UserInviteMap : EntityTypeConfiguration<TB_UserInvite>
    {
        public TB_UserInviteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.PhoneNo)
                .IsRequired()
                .HasMaxLength(20);

            this.Property(t => t.Ip)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("TB_UserInvite");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.PhoneNo).HasColumnName("PhoneNo");
            this.Property(t => t.NewUserId).HasColumnName("NewUserId");
            this.Property(t => t.CostAwardRate).HasColumnName("CostAwardRate");
            this.Property(t => t.CashoutAwardRate).HasColumnName("CashoutAwardRate");
            this.Property(t => t.Ip).HasColumnName("Ip");
            this.Property(t => t.InviteTime).HasColumnName("InviteTime");
        }
    }
}
