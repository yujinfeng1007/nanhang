using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsCheckMap : EntityTypeConfiguration<GoodsCheck>
    {
        public GoodsCheckMap()
        {
            Property(p => p.Subject).HasColumnName("F_Subject");
            // 字典
            Property(p => p.GoodsStatus).HasColumnName("F_GoodsStatus").HasColumnType("smallint");
            // 外键
            Property(p => p.StorageId).HasColumnName("F_StorageId");
            // 导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
            HasOptional(p => p.Storage).WithMany().HasForeignKey(p => p.StorageId);
        }
    }
}