using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class BuildingMap : EntityTypeConfiguration<Building>
    {
        public BuildingMap()

        {
            ToTable("zhxy_building");

            HasKey(p => p.Id); 

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.BuildingNo).HasColumnName("building_no");
          
        }
    }
}