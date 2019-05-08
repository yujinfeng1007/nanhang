using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class CourseMap : EntityTypeConfiguration<SchCourse>
    {
        public CourseMap()
        {
            ToTable("School_Course");
            HasKey(t => t.F_Id);
        }
    }
}