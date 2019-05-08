using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserLogOnMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLogOnMap()
        {
            ToTable("Sys_UserLogOn");
            HasKey(t => t.F_Id);
        }
    }
}