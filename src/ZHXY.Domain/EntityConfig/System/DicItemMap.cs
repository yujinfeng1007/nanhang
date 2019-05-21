using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DicItemMap : EntityTypeConfiguration<DicItem>
    {
        public DicItemMap()
        {
            ToTable("zhxy_dic_item");
            HasKey(p=>p.Id);

            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Key).HasColumnName("key");
            Property(p => p.Value).HasColumnName("value");
        }
    }
}