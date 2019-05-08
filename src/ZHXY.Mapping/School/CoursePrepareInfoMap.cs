using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class CoursePrepareInfoMap : EntityTypeConfiguration<PrepareLesson>
    {
        public CoursePrepareInfoMap()
        {
            ToTable("School_Course_Prepare_Info");
            HasKey(t => t.F_Id);
            HasOptional(p => p.Course).WithMany().HasForeignKey(p => p.F_Course).WillCascadeOnDelete(false);
        }
    }
}