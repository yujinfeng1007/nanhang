using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ItemsMap : EntityTypeConfiguration<SysDic>
    {
        public ItemsMap()
        {
            ToTable("zhxy_item");
            HasKey(t => t.F_Id);
        }
    }
}