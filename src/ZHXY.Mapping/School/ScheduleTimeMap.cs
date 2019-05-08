using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ScheduleTimeMap : EntityTypeConfiguration<SchScheduleTime>
    {
        public ScheduleTimeMap()
        {
            ToTable("School_Schedules_Time");
            HasKey(t => t.F_Id);
            HasOptional(p => p.Semester).WithMany().HasForeignKey(p => p.F_Semester).WillCascadeOnDelete(false);
        }
    }
}