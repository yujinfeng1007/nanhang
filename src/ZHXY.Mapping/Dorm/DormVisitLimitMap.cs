using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class DormVisitLimitMap : EntityTypeConfiguration<DormVisitLimit>
    {
        public DormVisitLimitMap()
        {
            ToTable("Dorm_Visit_Limit");
            HasKey(t => t.Student_Id);
        }
    }
}