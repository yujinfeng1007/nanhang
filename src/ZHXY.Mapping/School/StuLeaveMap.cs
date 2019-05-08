using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    /// <summary>
    /// 学生请假
    /// </summary>
    public class StuLeaveMap : EntityTypeConfiguration<StuLeaveOrder>
    {
        public StuLeaveMap()
        {
            ToTable("School_Stu_Leave");
            Property(p => p.ApplicantId).HasColumnName("F_Applicant").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.StartTime).HasColumnName("F_StartTime").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.EndOfTime).HasColumnName("F_EndTime").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveerId).HasColumnName("F_StudentID").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.HeadTeacherId).HasColumnName("F_TeacherID").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveDays).HasColumnName("F_LeaveDays").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveType).HasColumnName("F_LeaveType").HasColumnType("varchar");
            Property(p => p.ReasonForLeave).HasColumnName("F_ReasonForLeave").HasColumnType("varchar");
            Property(p => p.Status).HasColumnName("F_Status").HasColumnType("varchar");
            Property(p => p.ApprovalOpinion).HasColumnName("F_ApprovalOpinion").HasColumnType("varchar");


            // 导航属性
            HasOptional(p => p.Applicant).WithMany().HasForeignKey(p => p.ApplicantId);
            HasOptional(p => p.Leaveer).WithMany().HasForeignKey(p => p.LeaveerId);
            HasOptional(p => p.HeadTeacher).WithMany().HasForeignKey(p => p.HeadTeacherId);
        }
    }
}