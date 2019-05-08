using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIP>
    {
        public FilterIPMap()
        {
            ToTable("Sys_FilterIP");
            HasKey(t => t.F_Id);
        }
    }
}