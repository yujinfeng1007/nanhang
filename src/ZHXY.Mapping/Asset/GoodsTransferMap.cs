using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsTransferMap : EntityTypeConfiguration<GoodsTransfer>
    {
        public GoodsTransferMap()
        {
            // 字典
            Property(p => p.GoodsStatus).HasColumnName("F_GoodsStatus").HasColumnType("smallint");
            // 外键
            Property(p => p.ToStorageId).HasColumnName("F_ToStorageId");
            Property(p => p.FromStorageId).HasColumnName("F_FromStorageId");
            // 导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.FromStorage).WithMany().HasForeignKey(p => p.FromStorageId);
            HasOptional(p => p.ToStorage).WithMany().HasForeignKey(p => p.ToStorageId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}