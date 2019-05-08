using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Domain
{
    public class GateMap : EntityTypeConfiguration<Gate>
    {
        public GateMap()
        {
            ToTable("dorm_gate");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Name).HasColumnName("name");
            Property(p => p.DeviceNumber).HasColumnName("device_number");
            Property(p => p.Location).HasColumnName("location");
            Property(p => p.IP).HasColumnName("ip");
            Property(p => p.Mac).HasColumnName("mac");
            Property(p => p.Version).HasColumnName("version");
            Property(p => p.Status).HasColumnName("status");
        }
    }
}