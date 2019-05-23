using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ItemsDetailMap : EntityTypeConfiguration<SysDicItem>
    {
        public ItemsDetailMap()
        {
            ToTable("Sys_ItemsDetail");
            HasKey(t => t.F_Id);
        }
    }
}