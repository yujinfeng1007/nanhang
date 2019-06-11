using System;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 登录考核view
    /// </summary>
    public class LoginStatisticsView
    {

        public string UserId { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime? LastLoginTime { get; set; }
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes { get; set; }
        /// <summary>
        /// 未登录天数
        /// </summary>
        public int NoLoggedDays
        {
            get
            {
                return LastLoginTime.HasValue? (DateTime.Now - LastLoginTime.Value).Days:-1;
            }
        }

        public DateTime StartTime { get; set; }
        public DateTime EndOfTime { get; set; }
    }
}