using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserLoginMap : EntityTypeConfiguration<UserLogin>
    {
        public UserLoginMap()
        {
            ToTable("zhxy_user_login");
            HasKey(t => t.UserId);


            Property(p => p.UserId).HasColumnName("user_id");
            Property(p => p.Password).HasColumnName("password");
            Property(p => p.Secretkey).HasColumnName("secret_key");
            Property(p => p.PreVisitTime).HasColumnName("pre_visit_time");
            Property(p => p.LastVisitTime).HasColumnName("last_visit_time");
            Property(p => p.LoginCount).HasColumnName("login_count");
        }
    }
}