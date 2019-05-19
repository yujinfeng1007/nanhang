using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("zhxy_role");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("id");
            Property(p => p.OrganId).HasColumnName("organ_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Type).HasColumnName("type");
            Property(p => p.DataType).HasColumnName("data_type");
            Property(p => p.DataDeps).HasColumnName("data_deps");
            Property(p => p.SortCode).HasColumnName("sort_code");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}