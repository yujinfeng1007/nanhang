using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetCategoryMap : EntityTypeConfiguration<AssetCategory>
    {
        public AssetCategoryMap()
        {
            Property(p => p.Name).HasColumnName("F_FullName").HasColumnType("varchar").HasMaxLength(200);
            Property(p => p.Salvage).HasColumnName("F_Salvage").HasColumnType("decimal");
            Property(p => p.TimeLimit).HasColumnName("F_TimeLimit").HasColumnType("decimal");
            Property(p => p.Unit).HasColumnName("F_Unit").HasColumnType("varchar").HasMaxLength(50);

            // 外键
            Property(p => p.ParentId).HasColumnName("F_ParentID").HasColumnType("varchar").HasMaxLength(50);

            // 导航属性
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}