using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class BuildingMap : EntityTypeConfiguration<Building>
    {
        public BuildingMap()

        {
            ToTable("dorm_building");

            HasKey(p => p.Id); 

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Title).HasColumnName("title");
            Property(p => p.Area).HasColumnName("area");
            Property(p => p.BuildingNo).HasColumnName("building_no");
            Property(p => p.FloorNum).HasColumnName("floor_num");
            Property(p => p.UnitNum).HasColumnName("unit_num");
            Property(p => p.Address).HasColumnName("address");
            Property(p => p.BuildingType).HasColumnName("classroom_type");
            Property(p => p.BuildingStatus).HasColumnName("classroom_status");
        }
    }
}