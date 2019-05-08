using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsApplyDetailMap : EntityTypeConfiguration<GoodsApplyDetail>
    {
        public GoodsApplyDetailMap()
        {
            HasRequired(p => p.GoodsInfo).WithMany().HasForeignKey(p => p.GoodsId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}