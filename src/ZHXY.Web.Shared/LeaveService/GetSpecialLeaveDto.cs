using System;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 获取不计考勤请假列表
    /// </summary>
    public class GetSpecialLeaveDto
    {
        public string CurrentUserId { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public DateTime EndOfTime { get; set; }

        /// <summary>
        /// 分页索引
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        /// 分页大小
        /// </summary>
        public int Rows { get; set; } = 10;
    }
}