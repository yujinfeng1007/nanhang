using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ExamSignUpMap : EntityTypeConfiguration<ExamSignUp>
    {
        public ExamSignUpMap()
        {
            ToTable("School_ExamSignUp");
            HasKey(t => t.F_Id);
            HasRequired(t => t.School_Exam_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_ZK_Id)
                .WillCascadeOnDelete(false);
        }
    }
}