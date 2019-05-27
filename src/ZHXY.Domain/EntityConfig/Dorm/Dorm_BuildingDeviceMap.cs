using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.Dorm
{
    public class Dorm_BuildingDeviceMap: EntityTypeConfiguration<Device>
    {
        public Dorm_BuildingDeviceMap()
        {
            ToTable("zhxy_device");

            HasKey(p => p.Id);
        }
    }
}
