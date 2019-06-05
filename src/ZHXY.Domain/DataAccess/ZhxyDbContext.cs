using System.Data.Entity;
using System.Reflection;

namespace ZHXY.Domain
{
    public class ZhxyDbContext : DbContext
    {
       
        public ZhxyDbContext() : base("default")
        {
            Configuration.ValidateOnSaveEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(builder);
        }
    }
}