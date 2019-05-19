using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DicMap : EntityTypeConfiguration<SysDic>
    {
        public DicMap()
        {
            ToTable("zhxy_dic");
            HasKey(p => p.Id);

            Property(p => p.Category).HasColumnName("category");
            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.SortCode).HasColumnName("sort_code");

        }
    }
}