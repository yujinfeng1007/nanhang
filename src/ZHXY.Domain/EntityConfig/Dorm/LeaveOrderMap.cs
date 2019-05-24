using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    /// <summary>
    /// 学生请假
    /// </summary>
    public class LeaveOrderMap : EntityTypeConfiguration<LeaveOrder>
    {
        public LeaveOrderMap()
        {
            ToTable("zhxy_leave_order");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.CreatedTime).HasColumnName("created_time");
            Property(p => p.ApplicantId).HasColumnName("applicant_id");
            Property(p => p.StartTime).HasColumnName("start_time");
            Property(p => p.EndOfTime).HasColumnName("end_time");
            Property(p => p.LeaveerId).HasColumnName("leaveer_id");
            Property(p => p.HeadTeacherId).HasColumnName("head_teacher_id");
            Property(p => p.LeaveDays).HasColumnName("days");
            Property(p => p.LeaveType).HasColumnName("type");
            Property(p => p.Reason).HasColumnName("reason");
            Property(p => p.Status).HasColumnName("status");
            Property(p => p.Opinion).HasColumnName("opinion");
        }
    }
}