using ZHXY.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class UserRoleMap : EntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("zhxy_user_role");
            HasKey(p => new {p.UserId,p.RoleId });

            Property(p => p.UserId).HasColumnName("user_id");
            Property(p => p.RoleId).HasColumnName("role_id");
        }
    }
}