using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class OfficeTeacherMap : EntityTypeConfiguration<OfficeTeacher>
    {
        public OfficeTeacherMap()
        {
            ToTable("OfficeTeacher");
            HasKey(p => new { p.OfficeId, p.TeacherId });

            Property(p => p.OfficeId).HasColumnName("F_OfficeId");
            Property(p => p.TeacherId).HasColumnName("F_TeacherId");
        }
    }
}