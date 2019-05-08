using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ComeBackRouteMap : EntityTypeConfiguration<ComeBackRoute>
    {
        public ComeBackRouteMap()
        {
            ToTable("School_ComeBackRoute");
            HasKey(t => t.F_Id);
        }
    }
}