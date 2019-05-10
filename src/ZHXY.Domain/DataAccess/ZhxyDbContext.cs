using System.Configuration;
using System.Data.Entity;
using System.Reflection;
using ZHXY.Common;

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