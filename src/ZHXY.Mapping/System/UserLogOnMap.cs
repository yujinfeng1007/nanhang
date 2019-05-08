using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class UserLogOnMap : EntityTypeConfiguration<SysUserLogin>
    {
        public UserLogOnMap()
        {
            ToTable("Sys_UserLogOn");
            HasKey(t => t.F_Id);
        }
    }
}