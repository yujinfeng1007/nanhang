using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ComeBackRouteTwoMap : EntityTypeConfiguration<ComeBackRouteTwo>
    {
        public ComeBackRouteTwoMap()
        {
            ToTable("School_ComeBackRouteTwo");
            HasKey(t => t.F_Id);
        }
    }
}