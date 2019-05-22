using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class HolidayMap : EntityTypeConfiguration<Holiday>
    {
        public HolidayMap()
        {
            ToTable("zhxy_holiday");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.StartTime).HasColumnName("start_time");
            Property(p => p.EndTime).HasColumnName("end_time");
        }
    }
}
