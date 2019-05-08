using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{

    public class DormRepairMap : EntityTypeConfiguration<DormRepair>
    {
        public DormRepairMap()
        {
            ToTable("Dorm_Repair");
            HasKey(t => t.F_Id);
        }
    }
}