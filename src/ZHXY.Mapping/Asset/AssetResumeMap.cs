using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetResumeMap : EntityTypeConfiguration<AssetResume>
    {
        public AssetResumeMap()
        {
            Property(p => p.EventName).HasColumnName("F_EventName").HasColumnType("varchar");
            Property(p => p.EventTime).HasColumnName("F_EventTime").HasColumnType("datetime");
            Property(p => p.OrderType).HasColumnName("F_OrderType").HasColumnType("smallint");
            Property(p => p.OrderNumber).HasColumnName("F_OrderNumber").HasColumnType("varchar");
            Property(p => p.SortNumber).HasColumnName("F_SortNumber").HasColumnType("int");

            // 外键
            Property(p => p.AssetId).HasColumnName("F_AssetId");

            // 导航属性
            HasRequired(p => p.AssetInfo).WithMany().HasForeignKey(p => p.AssetId);
        }
    }
}