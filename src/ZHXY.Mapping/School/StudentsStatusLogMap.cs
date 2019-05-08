using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class StudentsStatusLogMap : EntityTypeConfiguration<StuStatus>
    {
        public StudentsStatusLogMap()
        {
            ToTable("School_Students_StatusLog");
            HasKey(t => t.F_Id);
        }
    }
}