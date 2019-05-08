using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Sys_Role");
            HasKey(t => t.F_Id);
        }
    }
}