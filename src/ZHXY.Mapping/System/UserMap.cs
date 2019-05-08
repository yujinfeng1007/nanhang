using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class UserMap : EntityTypeConfiguration<SysUser>
    {
        public UserMap()
        {
            ToTable("Sys_User");
            HasKey(t => t.F_Id);
        }
    }
}