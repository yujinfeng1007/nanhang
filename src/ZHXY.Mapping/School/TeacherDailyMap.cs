using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class TeacherDailyMap : EntityTypeConfiguration<TeacherDaily>
    {
        public TeacherDailyMap()
        {
            ToTable("School_Tearchers_Manage");
            HasKey(t => t.F_Id);
        }
    }
}