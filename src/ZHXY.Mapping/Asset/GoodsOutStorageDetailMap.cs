using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsOutStorageDetailMap : EntityTypeConfiguration<GoodsOutStorageDetail>
    {
        public GoodsOutStorageDetailMap()
        {
            // 导航
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}