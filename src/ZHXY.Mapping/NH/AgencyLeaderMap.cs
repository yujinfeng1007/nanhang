using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class AgencyLeaderMap : EntityTypeConfiguration<AgencyLeader>
    {
        public AgencyLeaderMap()
        {
            ToTable("School_AgencyLeader");

            HasKey(p => new { p.OrgId, p.UserId });

            Property(p => p.OrgId).HasColumnName("org_id");
            Property(p => p.UserId).HasColumnName("user_id");

        }
    }
}