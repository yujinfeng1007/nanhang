using ZHXY.Domain.Entity;
using System.Data.Entity.ModelConfiguration;

namespace ZHXY.Mapping
{
	 public class School_Alarm: EntityTypeConfiguration<Alarm>
    {
        public School_Alarm()
        {
            ToTable("School_Alarm");
            Property(p => p.AlarmUserId).HasColumnName("F_UserId");
            Property(p => p.Content).HasColumnName("F_Content");
            Property(p => p.AlarmType).HasColumnName("F_Type");
            Property(p => p.Status).HasColumnName("F_Status");
            Property(p => p.HandleUserId).HasColumnName("F_HandleUser");
            Property(p => p.AlarmUserName).HasColumnName("F_UserName");
            Property(p => p.HandleUserName).HasColumnName("F_HandleUserName");
        }
    }
}