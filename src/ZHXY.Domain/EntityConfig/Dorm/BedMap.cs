using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
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