
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ModuleMap : EntityTypeConfiguration<SysModule>
    {
        public ModuleMap()
        {
            ToTable("Sys_Module");
            HasKey(t => t.F_Id);
        }
    }
}