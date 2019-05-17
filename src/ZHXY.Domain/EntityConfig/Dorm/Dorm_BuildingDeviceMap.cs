using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.Dorm
{
    public class Dorm_BuildingDeviceMap: EntityTypeConfiguration<Dorm_BuildingDevice>
    {
        public Dorm_BuildingDeviceMap()
        {
            ToTable("Dorm_BuildingDevice");

            HasKey(p => p.Id);
        }
    }
}
