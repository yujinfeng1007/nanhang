using System.Data.Entity.ModelConfiguration;
using ZHXY.Domain.Entity.Dorm.Report;

namespace ZHXY.Domain.EntityConfig.Dorm.Report
{
    public class ZhxyPushMap: EntityTypeConfiguration<ZhxyPush>
    {
        public ZhxyPushMap()
        {
            ToTable("zhxy_push");
            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnName("id");
            Property(p => p.Account).HasColumnName("account");
            Property(p => p.CreateTime).HasColumnName("create_time");
            Property(p => p.Content).HasColumnName("content");
            Property(p => p.ReportDate).HasColumnName("report_date");
        }
    }
}
