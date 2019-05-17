using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class PlaceAreaMap : EntityTypeConfiguration<PlaceArea>
    {
        public PlaceAreaMap()
        {
            ToTable("zhxy_area");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("id");
            Property(p => p.ParentId).HasColumnName("p_id");
            Property(p => p.Level).HasColumnName("level");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.SimpleSpelling).HasColumnName("simple_spelling");
            Property(p => p.SortCode).HasColumnName("sort_code");
        }
    }
}