using System;

namespace ZHXY.Domain.Entity.Dorm.Report
{
    public class ZhxyPush: IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        public DateTime? CreateTime { get; set; }
        public string Content { get; set; }
        public string Account { get; set; }
        public DateTime? ReportDate { get; set; }
    }
}
