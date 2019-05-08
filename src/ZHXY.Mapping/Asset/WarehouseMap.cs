using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class WarehouseMap : EntityTypeConfiguration<Warehouse>
    {
        public WarehouseMap()
        {
            Property(p => p.Name).HasColumnName("F_FullName").HasColumnType("varchar");
            Property(p => p.Address).HasColumnName("F_Address").HasColumnType("varchar");

            // 字典属性
            Property(p => p.Status).HasColumnName("F_Status").HasColumnType("smallint");

            // 外键
            Property(p => p.ManagerId).HasColumnName("F_ManagerID").HasColumnType("varchar");
            Property(p => p.ParentId).HasColumnName("F_ParentID").HasColumnType("varchar");

            // 导航属性
            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.ParentId);
            HasOptional(p => p.Manager).WithMany().HasForeignKey(p => p.ManagerId);
        }
    }
}