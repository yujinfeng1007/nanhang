using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetPartMap : EntityTypeConfiguration<AssetPart>
    {
        public AssetPartMap()
        {
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.Model).HasColumnName("F_Model");
            Property(p => p.BodyBarcode).HasColumnName("F_BodyBarcode");
            Property(p => p.Num).HasColumnName("F_Num");
            // 外键
            Property(p => p.AssetId).HasColumnName("F_AssetID");

            // 导航
            HasRequired(p => p.AssetInfo).WithMany(p => p.Parts).HasForeignKey(p => p.AssetId);
        }
    }
}