using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class StudentStrictMap : EntityTypeConfiguration<StudentStrict>
    {
        public StudentStrictMap()
        {
            HasKey(p => p.Id);

            Property(p => p.StudentId).HasColumnName("F_Student_ID");
            Property(p => p.Score).HasColumnName("F_ScorePoint");
            Property(p => p.StrictType).HasColumnName("F_StrictType");
            Property(p => p.Type1).HasColumnName("F_Type1");
            Property(p => p.Type2).HasColumnName("F_Type2");
            Property(p => p.ScoreHisId).HasColumnName("F_ScoreHis_ID");
            Property(p => p.Status).HasColumnName("F_Status");

            HasOptional(p => p.Student).WithMany().HasForeignKey(p => p.StudentId);
            HasOptional(p => p.Creator).WithMany().HasForeignKey(p => p.CreatedByUserId);
        }
    }
}