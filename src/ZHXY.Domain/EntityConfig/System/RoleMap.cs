using ZHXY.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class RoleMap : EntityTypeConfiguration<SysRole>
    {
        public RoleMap()
        {
            ToTable("zhxy_role");
            HasKey(t => t.F_Id);
        }
    }
}