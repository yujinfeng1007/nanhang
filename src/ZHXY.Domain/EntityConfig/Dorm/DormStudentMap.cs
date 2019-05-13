using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class DormStudentMap : EntityTypeConfiguration<DormStudent>
    {
        public DormStudentMap()
        {
            ToTable("Dorm_DormStudent");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.StudentId).HasColumnName("F_Student_ID");
            Property(p => p.DormId).HasColumnName("F_DormId");
            Property(p => p.BedId).HasColumnName("F_Bed_ID");
            Property(p => p.InTime).HasColumnName("F_In_Time");
            Property(p => p.Gender).HasColumnName("F_Sex");



            HasRequired(t => t.DormInfo)
                .WithMany()
                .HasForeignKey(t => t.DormId);

            HasRequired(t => t.Student)
               .WithMany()
               .HasForeignKey(t => t.StudentId);
        }
    }
}