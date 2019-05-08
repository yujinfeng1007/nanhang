using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetTransferMap : EntityTypeConfiguration<AssetTransfer>
    {
        public AssetTransferMap()
        {
            // 外键属性
            Property(p => p.FromStorageId).HasColumnName("F_FromStorageId").HasColumnType("varchar");
            Property(p => p.ToStorageId).HasColumnName("F_ToStorageId").HasColumnType("varchar");

            // 导航属性
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);

            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
            HasOptional(p => p.FromStorage).WithMany().HasForeignKey(p => p.FromStorageId);
            HasOptional(p => p.ToStorage).WithMany().HasForeignKey(p => p.ToStorageId);
        }
    }
}