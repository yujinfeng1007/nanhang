using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class DormStudentMap : EntityTypeConfiguration<DormStudent>
    {
        public DormStudentMap()
        {
            ToTable("zhxy_dorm_student");
            HasKey(t => t.Id);


            Property(p => p.Id).HasColumnName("id");
            Property(p => p.StudentId).HasColumnName("student_id");
            Property(p => p.DormId).HasColumnName("dorm_id");
            Property(p => p.BedId).HasColumnName("bed_id");
            Property(p => p.InTime).HasColumnName("in_time");
            Property(p => p.Gender).HasColumnName("gender");
            Property(p => p.Description).HasColumnName("description");



            HasRequired(t => t.DormInfo)
                .WithMany()
                .HasForeignKey(t => t.DormId);

            HasRequired(t => t.Student)
               .WithMany()
               .HasForeignKey(t => t.StudentId);
        }
    }
}