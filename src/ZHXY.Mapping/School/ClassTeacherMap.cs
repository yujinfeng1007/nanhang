using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ClassTeacherMap : EntityTypeConfiguration<ClassTeacher>
    {
        public ClassTeacherMap()
        {
            ToTable("School_Class_Info_Teacher");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_Teachers_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_Teachers_F_Leader_Tea)
                .WithMany()
                .HasForeignKey(t => t.F_Leader_Tea)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_Teachers_F_Leader_Tea2)
                .WithMany()
                .HasForeignKey(t => t.F_Leader_Tea2)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_Course_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_CourseID)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_Class_Entity)
               .WithMany()
               .HasForeignKey(t => t.F_ClassID)
               .WillCascadeOnDelete(false);
        }
    }
}