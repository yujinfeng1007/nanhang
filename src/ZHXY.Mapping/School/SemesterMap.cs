using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SemesterMap : EntityTypeConfiguration<SchSemester>
    {
        public SemesterMap()
        {
            ToTable("School_Semester");
            HasKey(t => t.F_Id);
        }
    }
}