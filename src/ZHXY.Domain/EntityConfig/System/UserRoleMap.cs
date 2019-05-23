using ZHXY.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class UserRoleMap : EntityTypeConfiguration<SysUserRole>
    {
        public UserRoleMap()
        {
            ToTable("Sys_User_Role");
            HasKey(t => t.F_Id);
        }
    }
}