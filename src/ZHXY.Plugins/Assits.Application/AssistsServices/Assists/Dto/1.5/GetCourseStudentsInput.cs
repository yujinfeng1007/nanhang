using ZHXY.Application;
namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.5.获取课程学生信息
    /// </summary>
    public class GetCourseStudentsInput : BaseApiInput
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public string F_CourseId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type F_Type { get; set; }

        public enum Type : int
        {
            全部 = 0,
            已签到 = 1,
            未签到 = 2
        }
    }
}