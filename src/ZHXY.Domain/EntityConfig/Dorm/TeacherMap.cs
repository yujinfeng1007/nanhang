using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class TeacherMap : EntityTypeConfiguration<Teacher>
    {
        public TeacherMap()
        {

            ToTable("School_Teachers");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.UserId).HasColumnName("F_User_ID");
            Property(p => p.F_Divis_ID).HasColumnName("F_Divis_ID");
            Property(p => p.F_Name).HasColumnName("F_Name");














            HasRequired(t => t.teacherSysUser)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false);
        }
    }
}