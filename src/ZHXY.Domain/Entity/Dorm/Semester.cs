using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 学期
    /// </summary>
    public class Semester : IEntity
    {
        /// <summary>
        /// ID
        /// </summary>

        public string Id { get; set; }

        /// <summary>
        /// 学年度
        /// </summary>

        public string Year { get; set; }

        /// <summary>
        /// 学期名称
        /// </summary>

        public string Name { get; set; }

        /// <summary>
        /// 学期别名
        /// </summary>

        public string Nickname { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>

        public DateTime? StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>

        public DateTime? EndOfTime { get; set; }
    }
}