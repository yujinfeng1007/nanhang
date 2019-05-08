using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class OfficeMap : EntityTypeConfiguration<Office>
    {
        public OfficeMap()
        {
            ToTable("School_Office");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.ClassroomId).HasColumnName("F_ClassroomId");
            Property(p => p.GradeId).HasColumnName("F_GradeId");
            Property(p => p.OfficeNumber).HasColumnName("F_OfficeNumber");
            Property(p => p.Notice).HasColumnName("F_Notice");
            Property(p => p.Remark).HasColumnName("F_Remark");

            HasIndex(p => new { p.Id, p.ClassroomId }).IsUnique(true);//一个办公室只能绑定一个教室

            HasOptional(p => p.Classroom).WithMany().HasForeignKey(p => p.ClassroomId).WillCascadeOnDelete(false);
            HasOptional(p => p.Grade).WithMany().HasForeignKey(p => p.GradeId).WillCascadeOnDelete(false);
        }
    }
}