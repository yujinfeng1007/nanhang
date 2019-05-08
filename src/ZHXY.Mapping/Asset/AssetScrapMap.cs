using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetScrapMap : EntityTypeConfiguration<AssetScrap>
    {
        public AssetScrapMap()
        {
            // 字典属性
            Property(p => p.Way).HasColumnName("F_Way").HasColumnType("varchar");

            // 导航属性
            HasMany(p => p.Details).WithRequired(p => p.Order).HasForeignKey(p => p.OrderId);

            HasOptional(p => p.Approver).WithMany().HasForeignKey(p => p.ApproverId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}