using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ClassScoreMap : EntityTypeConfiguration<ClassEvaluation>
    {
        public ClassScoreMap()
        {
            HasKey(p => p.Id);

            Property(p => p.Year).HasColumnName("F_Year");
            Property(p => p.Semester).HasColumnName("F_Semester");
            Property(p => p.DivisId).HasColumnName("F_Divis_ID");
            Property(p => p.GradeId).HasColumnName("F_Grade_ID");
            Property(p => p.ClassId).HasColumnName("F_Class_ID");
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
        }
    }
}