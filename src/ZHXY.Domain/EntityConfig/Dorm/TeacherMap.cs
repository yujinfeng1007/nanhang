using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class TeacherMap : EntityTypeConfiguration<Teacher>
    {
        public TeacherMap()
        {

            ToTable("zhxy_teacher");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.UserId).HasColumnName("user_id");
            Property(p => p.OrganId).HasColumnName("organ_id");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.Gender).HasColumnName("gender");
            Property(p => p.JobNumber).HasColumnName("job_number");
            Property(p => p.CredType).HasColumnName("cred_type");
            Property(p => p.CredNumber).HasColumnName("cred_number");
            Property(p => p.FacePhoto).HasColumnName("face_photo");
            Property(p => p.MobilePhone).HasColumnName("mobile_phone");
            Property(p => p.EntryTime).HasColumnName("entry_time");
        }
    }
}