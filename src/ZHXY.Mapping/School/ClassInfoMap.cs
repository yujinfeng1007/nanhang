using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ClassInfoMap : EntityTypeConfiguration<ClassInfo>
    {
        public ClassInfoMap()
        {
            ToTable("School_Class_Info");
            HasKey(t => t.F_Id);
            HasOptional(t => t.Classroom)
               .WithMany()
               .HasForeignKey(t => t.F_Classroom)
               .WillCascadeOnDelete(false);

            HasRequired(t => t.Org)
              .WithMany()
              .HasForeignKey(t => t.F_ClassID)
              .WillCascadeOnDelete(false);

            HasRequired(t => t.School_Students_F_Leader_Stu)
             .WithMany()
             .HasForeignKey(t => t.F_Leader_Stu)
             .WillCascadeOnDelete(false);
        }
    }
}