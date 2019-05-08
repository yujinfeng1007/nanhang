using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ClassDutyMap : EntityTypeConfiguration<ClassDuty>
    {
        public ClassDutyMap()
        {
            ToTable("School_Class_Duty");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_Students_Entity)
               .WithMany()
               .HasForeignKey(t => t.F_Student)
               .WillCascadeOnDelete(false);
        }
    }
}