using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ParentStudentMap : EntityTypeConfiguration<StuParent>
    {
        public ParentStudentMap()
        {
            ToTable("School_ParStu_Num");
            HasKey(t => t.F_Id);
            HasOptional(t => t.Student)
                .WithMany()
                .HasForeignKey(t => t.F_Stu_Id)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.Parent)
                .WithMany()
                .HasForeignKey(t => t.F_ParentId)
                .WillCascadeOnDelete(false);
        }
    }
}