using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class ComeBackSiteMap : EntityTypeConfiguration<ComeBackSite>
    {
        public ComeBackSiteMap()
        {
            ToTable("School_ComeBackSite");
            HasKey(t => t.F_Id);
        }
    }
}