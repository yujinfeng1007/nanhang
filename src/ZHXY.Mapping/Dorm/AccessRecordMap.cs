using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity;

namespace ZHXY.Mapping
{
    public class AccessRecordMap : EntityTypeConfiguration<AccessRecord>
    {
        public AccessRecordMap()
        {
            ToTable("DHFLOW");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Code).HasColumnName("code");
            Property(p => p.UserName).HasColumnName("user_name");
            Property(p => p.Date).HasColumnName("date");
            Property(p => p.SwipDate).HasColumnName("swip_date");
            Property(p => p.ChannelCode).HasColumnName("channel_code");
            Property(p => p.ChannelName).HasColumnName("channel_name");
            Property(p => p.DepartmentCode).HasColumnName("department_code");
            Property(p => p.DepartmentName).HasColumnName("department_name");
            Property(p => p.CardNum).HasColumnName("card_num");
            Property(p => p.Tel).HasColumnName("tel");
            Property(p => p.Gender).HasColumnName("gender");
            Property(p => p.CardType).HasColumnName("card_type");
            Property(p => p.InOut).HasColumnName("in_out");
            Property(p => p.EventType).HasColumnName("event_type");
            Property(p => p.DeviceType).HasColumnName("device_type");
        }
    }
}
