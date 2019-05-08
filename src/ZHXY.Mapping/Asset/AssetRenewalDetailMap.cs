using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetRenewalDetailMap : EntityTypeConfiguration<AssetRenewalDetail>
    {
        public AssetRenewalDetailMap()
        {
            Property(p => p.MExtend).HasColumnName("F_MExtend").HasColumnType("int");

            // 导航属性
            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}