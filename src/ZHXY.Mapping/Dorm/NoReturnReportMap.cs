using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class NoReturnReportMap :EntityTypeConfiguration<NoReturnReport>
    {
        public NoReturnReportMap()
        {
            ToTable("Dorm_NoReturnReport");
            HasKey(p => p.F_Id);
            HasOptional(p => p.Class).WithMany().HasForeignKey(p => p.F_Class);
            HasOptional(p => p.Dorm).WithMany().HasForeignKey(p => p.F_Dorm);
        }
    }
}
