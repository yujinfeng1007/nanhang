using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.System
{
    public class AppVersionMap : EntityTypeConfiguration<AppVersion>
    {
        public AppVersionMap()
        {
            ToTable("zhxy_app_version");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Version).HasColumnName("version");
            Property(p => p.Url).HasColumnName("url");
            Property(p => p.Description).HasColumnName("description");
            Property(p => p.CreatedTime).HasColumnName("created_time");
        }
    }
}
