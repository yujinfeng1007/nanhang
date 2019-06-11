using System;
using ZHXY.Common;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 获取登录考核dto
    /// </summary>
    public class GetLoginStatisticsDto : Pagination
    {
        /// <summary>
        /// 0:自定义时间  1:本周 2:本月  
        /// </summary>
        public int Pattern { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string OrgId { get; set; }
        public string Keyword { get; set; }
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