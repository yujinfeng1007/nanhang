using ZHXY.Domain;
using System;
using ZHXY.Common;

namespace ZHXY.Application
{

    /// <summary>
    /// 报表服务
    /// </summary>
    public class ReportAppService : AppService
    {
        public ReportAppService(IZhxyRepository r) => R = r;

        /// <summary>
        /// 获取异常出入报表
        /// </summary>
        public void GetAbnormalAccess(GetAbnormalAccessDto dto)
        {
            




        }
    }

    /// <summary>
    /// 获取异常出入统计
    /// </summary>
    public class GetAbnormalAccessDto 
    {
        /// <summary>
        /// 0:自定义时间  1:本周 2:本月  
        /// </summary>
        public int Pattern { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string OrgId { get; set; }
        public DateTime StartTime
        {
            get
            {
                var d = DateTime.MinValue;
                switch (Pattern)
                {
                    case -1: DateTime.TryParse(Start, out d); break;
                    case 0: d = DateHelper.GetStartTimeOfWeek(); break;
                    case 1: d = DateHelper.GetStartTimeOfMonth(); break;
                }
                return d;
            }
        }
        public DateTime EndOfTime
        {
            get
            {
                var d = DateTime.Now;
                switch (Pattern)
                {
                    case -1: d = DateTime.TryParse(End, out d) ? d.AddDays(1).AddSeconds(-1) : DateTime.Now.Date.AddDays(1).AddSeconds(-1); break;
                    case 0: d = DateHelper.GetEndTimeOfWeek(); break;
                    case 1: d = DateHelper.GetEndTimeOfMonth(); break;
                }
                return d;
            }

        }
    }


}