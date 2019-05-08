using System.Data.Entity.ModelConfiguration;
using ZHXY.Assists.Entity;

namespace ZHXY.Assists.Mapping
{
    public class DevicesMap : EntityTypeConfiguration<DevicesEntity>
    {
        public DevicesMap()
        {
            ToTable("Assits_Devices");
            HasKey(t => t.F_Id);

            HasOptional(p => p.Classroom).WithMany().HasForeignKey(p => p.F_Classroom);
        }
    }
}