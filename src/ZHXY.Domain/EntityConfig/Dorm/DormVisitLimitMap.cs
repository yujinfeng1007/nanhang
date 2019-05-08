using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
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