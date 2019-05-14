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
            Property(p => p.OrganId).HasColumnName("F_Divis_ID");
            Property(p => p.Name).HasColumnName("F_Name");
            Property(p => p.Gender).HasColumnName("F_Gender");
            Property(p => p.JobNumber).HasColumnName("F_Num");
            Property(p => p.CredType).HasColumnName("F_CredType");
            Property(p => p.CredNum).HasColumnName("F_CredNum");
            Property(p => p.FacePhoto).HasColumnName("F_FacePhoto");
            Property(p => p.MobilePhone).HasColumnName("F_MobilePhone");
            Property(p => p.EntryTime).HasColumnName("F_EntryTime");
        }
    }
}