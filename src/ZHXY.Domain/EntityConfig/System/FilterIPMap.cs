using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class FilterIPMap : EntityTypeConfiguration<FilterIp>
    {
        public FilterIPMap()
        {
            ToTable("zhxy_filter_ip");
            HasKey(t => t.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Type).HasColumnName("type");
            Property(p => p.StartIp).HasColumnName("start_ip");
            Property(p => p.EndIp).HasColumnName("end_ip");
            Property(p => p.Description).HasColumnName("description");
        }
    }
}