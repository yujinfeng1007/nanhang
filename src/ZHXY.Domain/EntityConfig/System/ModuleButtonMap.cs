using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
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