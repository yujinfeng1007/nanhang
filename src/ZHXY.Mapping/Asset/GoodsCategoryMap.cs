using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsCategoryMap : EntityTypeConfiguration<GoodsCategory>
    {
        public GoodsCategoryMap()
        {
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.SearchIndex).HasColumnName("F_SearchIndex");
            Property(p => p.State).HasColumnName("F_State");
            Property(p => p.ParentId).HasColumnName("F_ParentID");
            // 导航
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}