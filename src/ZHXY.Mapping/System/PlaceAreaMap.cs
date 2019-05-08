using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class PlaceAreaMap : EntityTypeConfiguration<PlaceArea>
    {
        public PlaceAreaMap()
        {
            ToTable("Sys_Area");
            HasKey(t => t.F_Id);
        }
    }
}