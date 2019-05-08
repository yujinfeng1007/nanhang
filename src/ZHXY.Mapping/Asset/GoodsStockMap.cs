using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsStockMap : EntityTypeConfiguration<GoodsStock>
    {
        private GoodsStockMap()
        {
            Property(p => p.Total).HasColumnName("F_Total");
            Property(p => p.LockQuantity).HasColumnName("F_LockQuantity");
            // 导航
            HasRequired(p => p.StorePlace).WithMany().HasForeignKey(p => p.StorageId);
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
        }
    }
}