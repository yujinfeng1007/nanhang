using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIp>
    {
        public FilterIPMap()
        {
            ToTable("Sys_FilterIP");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("F_Id");
            Property(p => p.Type).HasColumnName("F_Type");
            Property(p => p.StartWithIp).HasColumnName("F_StartIP");
            Property(p => p.EndWithIp).HasColumnName("F_EndIP");
            Property(p => p.Description).HasColumnName("F_Description");
        }
    }
}