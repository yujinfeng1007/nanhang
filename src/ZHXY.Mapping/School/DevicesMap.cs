using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class DevicesMap : EntityTypeConfiguration<ElectronicBoard>
    {
        public DevicesMap()
        {
            ToTable("School_Devices");
            HasKey(t => t.F_Id);
            //HasOptional(t => t.School_Classroom_Entity)
            //    .WithMany()
            //    .HasForeignKey(t => t.F_Classroom)
            //    .WillCascadeOnDelete(false);
            //HasOptional(t => t.School_Class)
            //    .WithMany()
            //    .HasForeignKey(t => t.F_Class)
            //    .WillCascadeOnDelete(false);
            //HasOptional(t => t.School_Class_Info_Entity)
            //    .WithMany()
            //    .HasForeignKey(t => t.F_Class)
            //    .WillCascadeOnDelete(false);
        }
    }
}