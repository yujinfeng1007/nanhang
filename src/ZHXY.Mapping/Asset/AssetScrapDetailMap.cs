using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetScrapDetailMap : EntityTypeConfiguration<AssetScrapDetail>
    {
        public AssetScrapDetailMap()
        {
            // 字典属性
            Property(p => p.ValuesLeft).HasColumnName("F_ValuesLeft").HasColumnType("decimal");

            // 导航属性
            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}