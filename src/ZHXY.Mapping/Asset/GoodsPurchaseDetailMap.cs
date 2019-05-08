using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsPurchaseDetailMap : EntityTypeConfiguration<GoodsPurchaseDetail>
    {
        public GoodsPurchaseDetailMap()
        {
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice");
            // 导航
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
            HasRequired(p => p.Order).WithMany(p => p.Details).HasForeignKey(p => p.OrderId);
        }
    }
}