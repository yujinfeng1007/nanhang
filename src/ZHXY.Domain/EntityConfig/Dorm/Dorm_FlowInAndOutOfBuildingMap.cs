using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain.EntityConfig.Dorm
{
    public class Dorm_FlowInAndOutOfBuildingMap: EntityTypeConfiguration<Dorm_FlowInAndOutOfBuilding>
    {
        public Dorm_FlowInAndOutOfBuildingMap()
        {
            ToTable("Dorm_FlowInAndOutOfBuilding");

            HasKey(p => new { p.flow_id});
        }
    }
}
