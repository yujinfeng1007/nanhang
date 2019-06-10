using ZHXY.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class UserRoleMap : EntityTypeConfiguration<SysUserRole>
    {
        public UserRoleMap()
        {
            ToTable("zhxy_user_role");
            HasKey(t => t.F_Id);
        }
    }
}