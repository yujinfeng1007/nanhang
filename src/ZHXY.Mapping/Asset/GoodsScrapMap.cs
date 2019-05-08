using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsScrapMap : EntityTypeConfiguration<GoodsScrap>
    {
        public GoodsScrapMap()
        {
            // 字典
            Property(p => p.ScrapWay).HasColumnName("F_Way");
            Property(p => p.ScrapReason).HasColumnName("F_ScrapReason");
            Property(p => p.GoodsStatus).HasColumnName("F_GoodsStatus").HasColumnType("smallint");
            // 外键
            Property(p => p.StorageId).HasColumnName("F_StorageId");
            // 导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Storage).WithMany().HasForeignKey(p => p.StorageId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}