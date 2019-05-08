using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsInStorageMap : EntityTypeConfiguration<GoodsInStorage>
    {
        public GoodsInStorageMap()
        {
            Property(p => p.SourceNumber).HasColumnName("F_SourceNumber");
            // 字典
            Property(p => p.GoodsStatus).HasColumnName("F_GoodsStatus").HasColumnType("smallint");
            Property(p => p.InStorageType).HasColumnName("F_InStorageType").HasColumnType("smallint");
            // 外键
            Property(p => p.StorageId).HasColumnName("F_StorageId");
            Property(p => p.SupplierId).HasColumnName("F_SupplierId");
            // 导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.StorePlace).WithMany().HasForeignKey(p => p.StorageId);
            HasOptional(p => p.Supplier).WithMany().HasForeignKey(p => p.SupplierId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
        }
    }
}