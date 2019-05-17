using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DicItemMap : EntityTypeConfiguration<SysDicItem>
    {
        public DicItemMap()
        {
            ToTable("zhxy_dic_item");
            HasKey(p=>new { p.DicId,p.Key});

            Property(p => p.DicId).HasColumnName("dic_id");
            Property(p => p.Key).HasColumnName("key");
            Property(p => p.Value).HasColumnName("value");
        }
    }
}