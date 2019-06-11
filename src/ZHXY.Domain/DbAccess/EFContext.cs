using System.Data.Entity;
using System.Reflection;

namespace ZHXY.Domain
{
    public class EFContext : DbContext
    {
       
        public EFContext() : base("default")
        {
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = true;
            Configuration.UseDatabaseNullSemantics = true;
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}