using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
    public class FilterIPMap : EntityTypeConfiguration<SysFilterIP>
    {
        public FilterIPMap()
        {
            ToTable("Sys_FilterIP");
            HasKey(t => t.F_Id);
        }
    }
}