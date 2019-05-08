using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class AccessStudentMap: EntityTypeConfiguration< AccessStudent>
    {
        public AccessStudentMap()
        {
            ToTable("Dorm_AccessStudent");
            HasKey(t => t.F_Id);

            HasRequired(t => t.Device)
               .WithMany()
               .HasForeignKey(t => t.F_DeviceId);

            HasOptional(t => t.Student)
               .WithMany()
               .HasForeignKey(t => t.F_UserId);

            HasOptional(t => t.Teacher)
               .WithMany()
               .HasForeignKey(t => t.F_UserId);
        }
    }
}