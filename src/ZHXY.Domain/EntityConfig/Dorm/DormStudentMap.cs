using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class DormStudentMap : EntityTypeConfiguration<DormStudent>
    {
        public DormStudentMap()
        {
            ToTable("Dorm_DormStudent");
            HasKey(t => t.F_Id);

            HasRequired(t => t.DormInfo)
                .WithMany()
                .HasForeignKey(t => t.F_DormId);

            HasRequired(t => t.Student)
               .WithMany()
               .HasForeignKey(t => t.F_Student_ID);
        }
    }
}