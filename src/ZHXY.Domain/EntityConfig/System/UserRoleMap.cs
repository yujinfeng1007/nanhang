using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("Sys_User_Role");
            HasKey(t => t.F_Id);
        }
    }
}