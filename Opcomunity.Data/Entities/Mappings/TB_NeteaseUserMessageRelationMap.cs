using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Data.Entities
{
    public class TB_NeteaseUserMessageRelationMap : EntityTypeConfiguration<TB_NeteaseUserMessageRelation>
    {
        public TB_NeteaseUserMessageRelationMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("TB_NeteaseUserMessageRelation");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.MessageId).HasColumnName("MessageId");
            this.Property(t => t.FinishTime).HasColumnName("FinishTime");
        }
    }
}
