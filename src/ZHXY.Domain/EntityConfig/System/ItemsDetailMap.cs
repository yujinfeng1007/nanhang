using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
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