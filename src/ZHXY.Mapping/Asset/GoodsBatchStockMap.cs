using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsBatchStockMap : EntityTypeConfiguration<GoodsBatchStock>
    {
        private GoodsBatchStockMap()
        {
            Property(p => p.Total).HasColumnName("F_Total");
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice");
            Property(p => p.Date).HasColumnName("F_Date");
            // 导航
            HasRequired(p => p.StorePlace).WithMany().HasForeignKey(p => p.StorageId);
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
        }
    }
}