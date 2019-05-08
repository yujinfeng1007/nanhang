using System;

namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.6.获取课堂最新问题
    /// </summary>
    public partial class GetCourseLatestQuestionsOutput
    {
        /// <summary>
        /// 问题Id
        /// </summary>
        public string F_Id { get; set; }

        /// <summary>
        /// 问题
        /// </summary>
        public string F_Title { get; set; }

        /// <summary>
        /// 提问时间
        /// </summary>
        public DateTime? F_Time { get; set; }

        /// <summary>
        /// 提问者
        /// </summary>
        public StudentDto F_Student { get; set; }
    }
}