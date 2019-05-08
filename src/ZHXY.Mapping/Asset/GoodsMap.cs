using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GoodsMap : EntityTypeConfiguration<Goods>
    {
        public GoodsMap()
        {
            Property(p => p.Code).HasColumnName("F_GoodsCode");
            Property(p => p.Name).HasColumnName("F_FullName");
            Property(p => p.BarCode).HasColumnName("F_Barcode");
            Property(p => p.Brand).HasColumnName("F_Brand");
            Property(p => p.Size).HasColumnName("F_Size");
            Property(p => p.Origin).HasColumnName("F_Origin");
            Property(p => p.Model).HasColumnName("F_Model");
            Property(p => p.UnitPrice).HasColumnName("F_UnitPrice");
            Property(p => p.ValidLimit).HasColumnName("F_ValidLimit");
            Property(p => p.Least).HasColumnName("F_Least");
            Property(p => p.Most).HasColumnName("F_Most");
            Property(p => p.SearchIndex).HasColumnName("F_SearchIndex");
            // 字典
            Property(p => p.Status).HasColumnName("F_Status").HasColumnType("smallint").IsRequired();
            // 外键
            Property(p => p.CategoryId).HasColumnName("F_Type");
            Property(p => p.DefaultStorageId).HasColumnName("F_DefaultStorageId");
            // 导航
            HasOptional(p => p.Category).WithMany(p => p.GoodsInfos).HasForeignKey(p => p.CategoryId);
            HasOptional(p => p.DefaultStorage).WithMany().HasForeignKey(p => p.DefaultStorageId);
        }
    }
}