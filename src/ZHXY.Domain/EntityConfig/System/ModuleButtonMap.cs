
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ModuleButtonMap : EntityTypeConfiguration<SysButton>
    {
        public ModuleButtonMap()
        {
            ToTable("zhxy_module_button");
            HasKey(t => t.F_Id);
        }
    }
}