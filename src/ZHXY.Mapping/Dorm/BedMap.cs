using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class BedMap: EntityTypeConfiguration< DormBed>
    {
        public BedMap()
        {
            ToTable("Dorm_Bed");
            HasKey(t => t.F_Id);
        }
    }
}