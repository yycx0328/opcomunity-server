using System.Data.Entity;
using Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using System;
using System.Data.Entity.ModelConfiguration;

namespace Opcomunity.Passport.Entities
{
    public partial class PassportContext : DbContext
    {
        static PassportContext()
        {
            Database.SetInitializer<PassportContext>(null);
        }

        public PassportContext()
            : base("Name=PassportContext")
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

        public DbSet<TB_ApplicationInfo> TB_ApplicationInfo { get; set; } 
        public DbSet<TB_User> TB_User { get; set; }
        public DbSet<TB_UserAuth> TB_UserAuth { get; set; }
    }
}
