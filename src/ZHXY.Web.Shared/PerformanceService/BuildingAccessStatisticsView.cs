using System;

namespace ZHXY.Web.Shared
{
    public class BuildingAccessStatisticsView
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 院系
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 楼栋号
        /// </summary>
        public string BuildingId { get; set; }

        /// <summary>
        /// 楼栋号
        /// </summary>
        public string BuildingTitle { get; set; }
        /// <summary>
        /// 出入总次数
        /// </summary>
        public int VisitsCount { get; set; }

        /// <summary>
        /// 最后巡检时间
        /// </summary>
        public DateTime? LastInspectionTime { get; set; }

    }
}