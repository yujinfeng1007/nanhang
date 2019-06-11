using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class DicMap : EntityTypeConfiguration<Dic>
    {
        public DicMap()
        {
            ToTable("zhxy_dic");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
        }
    }
}