using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            ToTable("School_Students");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.YearNumber).HasColumnName("F_Year");
            Property(p => p.DivisId).HasColumnName("F_Divis_ID");
            Property(p => p.GradeId).HasColumnName("F_Grade_ID");
            Property(p => p.ClassId).HasColumnName("F_Class_ID");
            Property(p => p.SubjectId).HasColumnName("F_Subjects_ID");
            Property(p => p.StudentNumber).HasColumnName("F_StudentNum");
            Property(p => p.InitDTM).HasColumnName("F_InitDTM");
            Property(p => p.UserId).HasColumnName("F_Users_ID");
            Property(p => p.CardNumber).HasColumnName("F_Mac_No");
            Property(p => p.CurStatu).HasColumnName("F_CurStatu");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.Gender).HasColumnName("F_Gender");
            Property(p => p.CredType).HasColumnName("F_CredType");
            Property(p => p.CredNumber).HasColumnName("F_CredNum");
            Property(p => p.PolitStatu).HasColumnName("F_PolitStatu");
            Property(p => p.MobilePhone).HasColumnName("F_Tel");
            Property(p => p.OrganId).HasColumnName("F_DepartmentId");
            Property(p => p.InOut).HasColumnName("F_InOut");
        }
    }
}