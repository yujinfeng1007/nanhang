using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class CourseTeacherMap : EntityTypeConfiguration<TeacherCourse>
    {
        public CourseTeacherMap()
        {
            ToTable("School_Course_Teacher");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_Teachers_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher)
                .WillCascadeOnDelete(false);

            HasOptional(t => t.School_Course_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_CourseID)
                .WillCascadeOnDelete(false);
        }
    }
}