using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class FlowRedFlagScoreMap : EntityTypeConfiguration<MobileRedFlagScore>
    {
        public FlowRedFlagScoreMap()
        {
            HasKey(p => p.Id);
            Property(p => p.Year).HasColumnName("F_Year");
            Property(p => p.Semester).HasColumnName("F_Semester");
            Property(p => p.DivisId).HasColumnName("F_Divis_ID");
            Property(p => p.GradeId).HasColumnName("F_Grade_ID");
            Property(p => p.ClassId).HasColumnName("F_Class_ID");
            Property(p => p.AuditorTime).HasColumnName("F_Auditor_Time");
            Property(p => p.AuditorMemo).HasColumnName("F_Auditor_Memo");
            Property(p => p.Imgs).HasColumnName("F_Imgs");

            HasOptional(p => p.Auditor).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}