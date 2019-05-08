using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SheetCodeMap : EntityTypeConfiguration<SheetCode>
    {
        public SheetCodeMap()
        {
            HasKey(p => p.TableName);

            Property(p => p.TableName).HasColumnName("TableName").HasColumnType("varchar");
            Property(p => p.FlagCode).HasColumnName("FlagCode").HasColumnType("varchar");
            Property(p => p.Date).HasColumnName("F_Date").HasColumnType("varchar");
            Property(p => p.Number).HasColumnName("F_Number").HasColumnType("int");
        }
    }
}