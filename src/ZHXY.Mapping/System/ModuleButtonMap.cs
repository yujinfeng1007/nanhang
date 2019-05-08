using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ModuleButtonMap : EntityTypeConfiguration<SysButton>
    {
        public ModuleButtonMap()
        {
            ToTable("Sys_ModuleButton");
            HasKey(t => t.F_Id);
        }
    }
}