using Infrastructure;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
namespace Opcomunity.Data.Entities
{
    public class OpcomunityContext : DbContext
    {
        static OpcomunityContext()
        {
            Database.SetInitializer<OpcomunityContext>(null);
        }

        public OpcomunityContext() : base("name=OpcomunityContext")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var mappings = GetType().Assembly.GetInheritedTypes(typeof(EntityTypeConfiguration<>));
            foreach (var mapping in mappings)
            {
                dynamic instance = Activator.CreateInstance(mapping);
                modelBuilder.Configurations.Add(instance);
            }
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var item in error.ValidationErrors)
                    {
                        sb.AppendLine(item.PropertyName + ": " + item.ErrorMessage);
                    }
                }
                LogHelper.TryLog("SaveChanges.DbEntityValidation", ex.GetAllMessages() + sb);
                throw;
            }
        }

        public DbSet<TB_OrderAlipayCallbackResult> TB_OrderAlipayCallbackResult { get; set; }
        public DbSet<TB_OrderCharge> TB_OrderCharge { get; set; }
    }
}