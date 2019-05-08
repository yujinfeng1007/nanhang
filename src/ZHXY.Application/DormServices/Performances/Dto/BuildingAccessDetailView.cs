using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 进出楼栋详情view
    /// </summary>
    public class BuildingAccessDetailView
    {
        public string Name { get; set; }
        public string Department { get; set; }
        public string BuildingTitle { get; set; }
        public bool Direction { get; set; }
        public DateTime? Time { get; set; }
    }
}