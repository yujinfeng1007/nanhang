using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class BuildingDeviceMap : EntityTypeConfiguration<BuildingDevice>
    {
        public BuildingDeviceMap()
        {
            ToTable("Dorm_BuildingDevice");
            HasKey(p =>new {p.BuildingId,p.DeviceId });

            Property(p => p.BuildingId).HasColumnName("building_id");
            Property(p => p.DeviceId).HasColumnName("device_id");
        }
    }
}