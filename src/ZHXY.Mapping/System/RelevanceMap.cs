using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class RelevanceMap : EntityTypeConfiguration<Relevance>
    {
        public RelevanceMap()
        {
            ToTable("sys_relevance");
            HasKey(p => new { p.Name ,p.FirstKey,p.SecondKey,p.ThirdKey});

            Property(p => p.Name).HasColumnName("name");
            Property(p => p.FirstKey).HasColumnName("first_key");
            Property(p => p.SecondKey).HasColumnName("second_key");
            Property(p => p.ThirdKey).HasColumnName("third_key");
        }
    }
}