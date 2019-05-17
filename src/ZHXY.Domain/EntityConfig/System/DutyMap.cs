using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class DutyMap : EntityTypeConfiguration<Duty>
    {
        public DutyMap()
        {
            ToTable("zhxy_duty");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.SortCode).HasColumnName("sort_code");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}