using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class SysOrgLeaderMap : EntityTypeConfiguration<OrgLeader>
    {
        public SysOrgLeaderMap()
        {
            ToTable("zhxy_org_leader");
            HasKey(t => new { t.OrgId, t.UserId });

            Property(p => p.OrgId).HasColumnName("org_id");
            Property(p => p.UserId).HasColumnName("user_id");

            HasRequired(p => p.Org).WithMany().HasForeignKey(p => p.OrgId);
            HasRequired(p => p.User).WithMany().HasForeignKey(p => p.UserId);
        }
    }
}