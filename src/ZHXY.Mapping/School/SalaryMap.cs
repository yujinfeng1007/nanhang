using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SalaryMap : EntityTypeConfiguration<TeacherSalary>
    {
        public SalaryMap()
        {
            ToTable("School_Salary");
            HasKey(t => t.F_Id);
        }
    }
}