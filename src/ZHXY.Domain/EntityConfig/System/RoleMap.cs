using ZHXY.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("zhxy_role");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.DataType).HasColumnName("data_type");
            Property(p => p.DataDeps).HasColumnName("data_deps");
            Property(p => p.Sort).HasColumnName("sort");
        }
    }
}