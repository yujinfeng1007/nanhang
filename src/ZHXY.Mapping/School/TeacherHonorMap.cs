using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class TeacherHonorMap : EntityTypeConfiguration<TeacherHonor>
    {
        public TeacherHonorMap()
        {
            ToTable("School_Teacher_Honor");
            HasKey(t => t.F_Id);
        }
    }
}