using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class AttendanceStuFlowMap : EntityTypeConfiguration<AttendanceStuFlow>
    {
        public AttendanceStuFlowMap()
        {
            ToTable("School_Attendance_Stu_Flow");
            HasKey(t => t.F_Id);
        }
    }
}