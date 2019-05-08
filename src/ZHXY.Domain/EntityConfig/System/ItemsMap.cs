using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class ItemsMap : EntityTypeConfiguration<SysDic>
    {
        public ItemsMap()
        {
            ToTable("Sys_Items");
            HasKey(t => t.F_Id);
        }
    }
}