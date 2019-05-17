using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            ToTable("zhxy_student");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.YearNumber).HasColumnName("year");
            Property(p => p.DivisId).HasColumnName("divis_id");
            Property(p => p.GradeId).HasColumnName("grade_id");
            Property(p => p.ClassId).HasColumnName("class_id");
            Property(p => p.SubjectId).HasColumnName("subject_id");
            Property(p => p.StudentNumber).HasColumnName("student_number");
            Property(p => p.InitDTM).HasColumnName("init_dtm");
            Property(p => p.UserId).HasColumnName("user_id");
            Property(p => p.CardNumber).HasColumnName("card_number");
            Property(p => p.CurStatu).HasColumnName("cur_statu");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Gender).HasColumnName("gender");
            Property(p => p.CredType).HasColumnName("cred_type");
            Property(p => p.CredNumber).HasColumnName("cred_number");
            Property(p => p.PolitStatu).HasColumnName("PolitStatu");
            Property(p => p.MobilePhone).HasColumnName("mobile_phone");
            Property(p => p.OrganId).HasColumnName("organ_id");
        }
    }
}