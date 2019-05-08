using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class InOutReceiveMap: EntityTypeConfiguration< InOutReceive>
    {
        public InOutReceiveMap()
        {
            ToTable("Dorm_InOutReceive");
            HasKey(t => t.F_Id);
        }
    }
}