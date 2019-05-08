using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class SysOrgMap : EntityTypeConfiguration<Organize>
    {
        public SysOrgMap()
        {
            ToTable("Sys_Organize");
            HasKey(t => t.F_Id);

            HasOptional(p => p.Parent).WithMany(p => p.Children).HasForeignKey(p => p.F_ParentId);
        }
    }
}