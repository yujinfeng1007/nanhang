using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ClassroomMap : EntityTypeConfiguration<Classroom>
    {
        public ClassroomMap()
        {
            ToTable("School_Classroom");
            HasKey(t => t.F_Id);
        }
    }
}