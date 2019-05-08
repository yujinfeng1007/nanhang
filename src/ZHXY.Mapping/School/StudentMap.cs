using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class StudentMap : EntityTypeConfiguration<Student>
    {
        public StudentMap()
        {
            ToTable("School_Students");
            HasKey(t => t.F_Id);
            HasRequired(t => t.studentSysUser)
                .WithMany()
                .HasForeignKey(t => t.F_Users_ID)
                .WillCascadeOnDelete(false);
        }
    }
}