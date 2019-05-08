using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class TeacherChangeMap : EntityTypeConfiguration<TeacherChange>
    {
        public TeacherChangeMap()
        {
            ToTable("School_Teachers_Change");
            HasKey(t => t.F_Id);
        }
    }
}