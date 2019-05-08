using ZHXY.Application;
namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.6.获取课堂最新问题
    /// </summary>
    public class GetCourseLatestQuestionsInput : BaseApiInput
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public string F_CourseId { get; set; }

        /// <summary>
        /// 最新条数
        /// </summary>
        public int F_LatestCount { get; set; }
    }
}