using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsApplyMap : EntityTypeConfiguration<GoodsApply>
    {
        public GoodsApplyMap()
        {
            //字典
            Property(p => p.Leave).HasColumnName("F_Leave");
            Property(p => p.GoodsStatus).HasColumnName("F_GoodsStatus").HasColumnType("smallint");
            //外键
            Property(p => p.UserId).HasColumnName("F_UserID");
            Property(p => p.UseDepartmentId).HasColumnName("F_Department");
            Property(p => p.StorageId).HasColumnName("F_StorageId");
            //导航
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);
            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.User).WithMany().HasForeignKey(p => p.UserId);
            HasOptional(p => p.UseDepartment).WithMany().HasForeignKey(p => p.UseDepartmentId);
            HasOptional(p => p.Storage).WithMany().HasForeignKey(p => p.StorageId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}