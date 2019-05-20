using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DicItemMap : EntityTypeConfiguration<SysDicItem>
    {
        public DicItemMap()
        {
            ToTable("zhxy_dic_item");
            HasKey(p=>p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Key).HasColumnName("key");
            Property(p => p.Value).HasColumnName("value");
        }
    }
}