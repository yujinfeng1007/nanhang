using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ScreenExamTypeMap : EntityTypeConfiguration<ExaminationMode>
    {
        public ScreenExamTypeMap()
        {
            ToTable("School_Screen_Exam_Type");
            HasKey(t => t.F_Id);
            HasOptional(t => t.School_Devices_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Devices)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.School_Classroom_Entity)
                .WithMany()
                .HasForeignKey(t => t.F_Classroom)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.Teacher1)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher1)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.Teacher2)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher2)
                .WillCascadeOnDelete(false);
            HasOptional(t => t.Teacher3)
                .WithMany()
                .HasForeignKey(t => t.F_Teacher3)
                .WillCascadeOnDelete(false);
        }
    }
}