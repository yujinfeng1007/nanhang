using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class StudentHonorMap : EntityTypeConfiguration<StuHonor>
    {
        public StudentHonorMap()
        {
            ToTable("School_Stu_Honor");
            HasKey(t => t.F_Id);
        }
    }
}