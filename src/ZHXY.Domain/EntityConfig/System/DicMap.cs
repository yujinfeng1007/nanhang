using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DicMap : EntityTypeConfiguration<Dic>
    {
        public DicMap()
        {
            ToTable("zhxy_dic");
            HasKey(p => p.Code);

            Property(p => p.Type).HasColumnName("type");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.SortCode).HasColumnName("sort_code");


        }
    }
}