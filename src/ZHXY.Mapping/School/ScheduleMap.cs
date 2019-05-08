using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ScheduleMap : EntityTypeConfiguration<ClassSchedule>
    {
        public ScheduleMap()
        {
            ToTable("School_Schedules");
            HasKey(t => t.F_Id);
        }
    }
}