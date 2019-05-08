using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class StudentScoreMap : EntityTypeConfiguration<StudentEvaluation>
    {
        public StudentScoreMap()
        {
            HasKey(p => p.Id);
            Property(p => p.StudentId).HasColumnName("F_Student_ID");
            Property(p => p.Type1).HasColumnName("F_Type1");
            Property(p => p.Type2).HasColumnName("F_Type2");
            Property(p => p.Score).HasColumnName("F_Score");
            Property(p => p.Imgs).HasColumnName("F_Imgs");
            Property(p => p.AuditorId).HasColumnName("F_Auditor");
            Property(p => p.AuditorTime).HasColumnName("F_Auditor_Time");
            Property(p => p.AuditorMemo).HasColumnName("F_Auditor_Memo");
            Property(p => p.AuditorScore).HasColumnName("F_Auditor_Score");
            Property(p => p.Status).HasColumnName("F_Status");

            HasOptional(p => p.Auditor).WithMany().HasForeignKey(p => p.AuditorId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
            HasOptional(p => p.Student).WithMany().HasForeignKey(p => p.StudentId);
        }
    }
}