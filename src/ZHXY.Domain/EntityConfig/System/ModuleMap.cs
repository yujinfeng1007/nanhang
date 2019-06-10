
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ModuleMap : EntityTypeConfiguration<SysModule>
    {
        public ModuleMap()
        {
            ToTable("zhxy_module");
            HasKey(t => t.F_Id);
        }
    }
}