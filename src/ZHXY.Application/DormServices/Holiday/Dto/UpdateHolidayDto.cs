using System;

namespace ZHXY.Application
{
    public class UpdateHolidayDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
