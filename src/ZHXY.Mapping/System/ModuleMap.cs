using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
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