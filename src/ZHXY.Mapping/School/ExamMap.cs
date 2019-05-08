using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ExamMap : EntityTypeConfiguration<RecruitmentType>
    {
        public ExamMap()
        {
            ToTable("School_Exam");
            HasKey(t => t.F_Id);
        }
    }
}