using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class HolidayMap : EntityTypeConfiguration<Holiday>
    {
        public HolidayMap()
        {
            ToTable("dorm_holiday");
            HasKey(p => p.Id);
       }
    }
}
