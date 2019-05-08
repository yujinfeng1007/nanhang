using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetOwnerMap : EntityTypeConfiguration<OwnerUnit>
    {
        public AssetOwnerMap()
        {
            Property(p => p.Name).HasColumnName("F_FullName").HasColumnType("varchar").HasMaxLength(200);
            Property(p => p.ShortName).HasColumnName("F_ShortName").HasColumnType("varchar");

            // 外键
            Property(p => p.ParentId).HasColumnName("F_ParentID").HasColumnType("varchar").HasMaxLength(50);

            // 导航属性
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
        }
    }
}