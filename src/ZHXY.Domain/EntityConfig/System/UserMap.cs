using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("Sys_User");
            HasKey(t => t.F_Id);

            Property(p => p.OrgId).HasColumnName("F_DepartmentId");
        }
    }
}