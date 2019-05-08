using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{

    public class RepairMap : EntityTypeConfiguration<DormRepair>
    {
        public RepairMap()
        {
            ToTable("Dorm_Repair");
            HasKey(t => t.F_Id);
        }
    }
}