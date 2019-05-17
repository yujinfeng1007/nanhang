using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.System
{
    public class AppVersionMap : EntityTypeConfiguration<AppVersion>
    {
        public AppVersionMap()
        {
            ToTable("Sys_AppVersion");
            HasKey(t => t.Id);
        }
    }
}
