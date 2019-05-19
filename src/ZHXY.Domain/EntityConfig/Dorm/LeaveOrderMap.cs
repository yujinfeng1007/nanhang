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
            Property(p => p.ApplicantId).HasColumnName("applicant_id").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.StartTime).HasColumnName("start_time").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.EndOfTime).HasColumnName("end_time").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveerId).HasColumnName("leaveer_id").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.HeadTeacherId).HasColumnName("head_teacher_id").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveDays).HasColumnName("days").HasColumnType("varchar").HasMaxLength(50);
            Property(p => p.LeaveType).HasColumnName("type").HasColumnType("varchar");
            Property(p => p.Reason).HasColumnName("reason").HasColumnType("varchar");
            Property(p => p.Status).HasColumnName("status").HasColumnType("varchar");
            Property(p => p.Opinion).HasColumnName("opinion").HasColumnType("varchar");


            // 导航属性
            HasOptional(p => p.Applicant).WithMany().HasForeignKey(p => p.ApplicantId);
            HasOptional(p => p.Leaveer).WithMany().HasForeignKey(p => p.LeaveerId);
            HasOptional(p => p.HeadTeacher).WithMany().HasForeignKey(p => p.HeadTeacherId);
        }
    }
}