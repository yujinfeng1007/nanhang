using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class GateChannelMap : EntityTypeConfiguration<GateChannel>
    {
        public GateChannelMap()
        {
            ToTable("dorm_gate_channel");
            HasKey(p => new { p.GateId, p.ChannelCode });

            Property(p => p.GateId).HasColumnName("gate_id");
            Property(p => p.ChannelCode).HasColumnName("channel_code");
            Property(p => p.ChannelName).HasColumnName("channel_name");
        }
    }
}