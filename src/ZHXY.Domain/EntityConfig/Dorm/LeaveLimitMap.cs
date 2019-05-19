using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class LeaveLimitMap : EntityTypeConfiguration<LeaveLimit>
    {
        public LeaveLimitMap()
        {
            ToTable("zhxy_leave_limit");

            HasKey(p => new { p.SemesterId, p.StudentId });

            Property(p => p.SemesterId).HasColumnName("semester_id");
            Property(p => p.StudentId).HasColumnName("student_id");
            Property(p => p.UsedDays).HasColumnName("used_days");

        }
    }
}