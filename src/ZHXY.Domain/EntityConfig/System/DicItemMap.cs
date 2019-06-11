using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class DicItemMap : EntityTypeConfiguration<DicItem>
    {
        public DicItemMap()
        {
            ToTable("zhxy_dic_item");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ItemId).HasColumnName("dic_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Sort).HasColumnName("sort");
        }
    }
}