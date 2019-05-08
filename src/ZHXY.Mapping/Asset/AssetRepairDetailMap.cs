using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetRepairDetailMap : EntityTypeConfiguration<AssetRepairDetail>
    {
        public AssetRepairDetailMap()
        {
            Property(p => p.PartName).HasColumnName("F_PartName").HasColumnType("varchar");
            Property(p => p.Num).HasColumnName("F_Num").HasColumnType("int");
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice").HasColumnType("decimal");

            // 导航属性
            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
            HasRequired(p => p.Order).WithMany().HasForeignKey(p => p.OrderId);
        }
    }
}