using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsInStorageDetailMap : EntityTypeConfiguration<GoodsInStorageDetail>
    {
        public GoodsInStorageDetailMap()
        {
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice");
            // 导航
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}