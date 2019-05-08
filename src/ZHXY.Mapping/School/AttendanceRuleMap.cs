using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AttendanceRuleMap : EntityTypeConfiguration<AttendanceRule>
    {
        public AttendanceRuleMap()
        {
            ToTable("School_Attendance_Rules");
            HasKey(t => t.F_Id);
        }
    }
}