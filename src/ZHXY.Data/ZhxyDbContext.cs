using System.Configuration;
using System.Data.Entity;
using ZHXY.Common;
using ZHXY.Data.Initializers;
using ZHXY.Mapping;

namespace ZHXY.Data
{
    public class ZhxyDbContext : DbContext
    {
        public ZhxyDbContext(string schoolCode = null)
        {
            schoolCode = schoolCode ?? OperatorProvider.Current?.SchoolCode;
            Database.Connection.ConnectionString = string.IsNullOrWhiteSpace(schoolCode)? ConfigurationManager.ConnectionStrings["default"].ConnectionString:DbConnHelper.GetConnectionString(schoolCode);
            var initializer = new CreateTablesIfNotExist<ZhxyDbContext>();
            Database.SetInitializer(initializer);
            Configuration.ValidateOnSaveEnabled = false;
        }


        //public ZhxyDbContext() : base("default")
        //{
        //    var initializer = new CreateTablesIfNotExist<ZhxyDbContext>();
        //    Database.SetInitializer(initializer);
        //    Configuration.ValidateOnSaveEnabled = false;
        //}

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            var assembly = MappingModule.GetModuleAssembly();
            builder.Configurations.AddFromAssembly(assembly);
            foreach (var m in OtherMaps.ModelCreatingMaps)
            {
                builder.Configurations.Add(m);
            }
            base.OnModelCreating(builder);
        }
    }
}