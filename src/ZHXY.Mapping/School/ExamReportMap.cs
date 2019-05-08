using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ExamReportMap : EntityTypeConfiguration<ExamReport>
    {
        public ExamReportMap()
        {
            ToTable("School_ExamReport");
            HasKey(t => t.F_Id);
            HasRequired(t => t.School_ExamTitle_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Title)
                .WillCascadeOnDelete(false);
        }
    }
}