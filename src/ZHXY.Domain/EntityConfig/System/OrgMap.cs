using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class OrgMap : EntityTypeConfiguration<Org>
    {
        public OrgMap()
        {
            ToTable("zhxy_org");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.ShortName).HasColumnName("short_name");
            Property(p => p.Type).HasColumnName("org_type");
            Property(p => p.Sort).HasColumnName("sort");
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}