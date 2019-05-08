using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class GradeCourseMap : EntityTypeConfiguration<SchGradeCourse>
    {
        public GradeCourseMap()
        {
            ToTable("School_Grade_Course");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_Course_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_CourseId)
                .WillCascadeOnDelete(false);
        }
    }
}