using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class NoOutReportMap : EntityTypeConfiguration<NoOutReport>
    {
        public NoOutReportMap()
        {
            ToTable("Dorm_NoOutReport");
            HasKey(p => p.F_Id);
            HasOptional(p => p.Class).WithMany().HasForeignKey(p => p.F_Class);
            HasOptional(p => p.Dorm).WithMany().HasForeignKey(p => p.F_Dorm);
        }
    }
}
