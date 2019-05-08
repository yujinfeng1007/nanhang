using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AssetSupplierMap : EntityTypeConfiguration<AssetSupplier>
    {
        public AssetSupplierMap()
        {
            Property(p => p.ShortName).HasColumnName("F_ShortName").HasColumnType("varchar");
            Property(p => p.Contact).HasColumnName("F_Contact").HasColumnType("varchar");
            Property(p => p.Email).HasColumnName("F_Email").HasColumnType("varchar");
            Property(p => p.Web).HasColumnName("F_Web").HasColumnType("varchar");
            Property(p => p.Address).HasColumnName("F_Address").HasColumnType("varchar");
            Property(p => p.Fax).HasColumnName("F_Fax").HasColumnType("varchar");
            Property(p => p.Phone).HasColumnName("F_Phone").HasColumnType("varchar");
            Property(p => p.Company).HasColumnName("F_Company").HasColumnType("varchar");
            Property(p => p.Account).HasColumnName("F_Account").HasColumnType("varchar");
            Property(p => p.Bank).HasColumnName("F_Bank").HasColumnType("varchar");
            Property(p => p.Ein).HasColumnName("F_Ein").HasColumnType("varchar");
            Property(p => p.SearchIndex).HasColumnName("F_SearchIndex").HasColumnType("varchar");

            // 字典属性
            Property(p => p.Status).HasColumnName("F_State").HasColumnType("varchar");
            Property(p => p.Type).HasColumnName("F_Type").HasColumnType("varchar");
        }
    }
}