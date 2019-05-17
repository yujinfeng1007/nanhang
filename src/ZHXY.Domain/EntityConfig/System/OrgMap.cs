using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class OrgMap : EntityTypeConfiguration<Organ>
    {
        public OrgMap()
        {
            ToTable("zhxy_organ");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.EnCode).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.ShortName).HasColumnName("short_name");
            Property(p => p.CategoryId).HasColumnName("category_id");
            Property(p => p.SortCode).HasColumnName("sort_code");
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}