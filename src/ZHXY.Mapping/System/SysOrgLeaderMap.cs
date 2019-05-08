using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SysOrgLeaderMap : EntityTypeConfiguration<SysOrgLeader>
    {
        public SysOrgLeaderMap()
        {
            ToTable("sys_org_leader");
            HasKey(t => new { t.OrgId, t.UserId });

            Property(p => p.OrgId).HasColumnName("org_id");
            Property(p => p.UserId).HasColumnName("user_id");

            HasRequired(p => p.Org).WithMany().HasForeignKey(p => p.OrgId);
            HasRequired(p => p.User).WithMany().HasForeignKey(p => p.UserId);
        }
    }
}