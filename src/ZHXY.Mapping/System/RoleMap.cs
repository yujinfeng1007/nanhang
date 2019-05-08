using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class RoleMap : EntityTypeConfiguration<SysRole>
    {
        public RoleMap()
        {
            ToTable("Sys_Role");
            HasKey(t => t.F_Id);
        }
    }
}