using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class SysOrgMap : EntityTypeConfiguration<SysOrganize>
    {
        public SysOrgMap()
        {
            ToTable("Sys_Organize");
            HasKey(t => t.F_Id);

            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.F_ParentId);
        }
    }
}