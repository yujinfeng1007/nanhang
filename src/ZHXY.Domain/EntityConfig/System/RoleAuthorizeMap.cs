
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class RoleAuthorizeMap : EntityTypeConfiguration<SysRoleAuthorize>
    {
        public RoleAuthorizeMap()
        {
            ToTable("zhxy_role_authorize");
            HasKey(t => t.F_Id);
        }
    }
}