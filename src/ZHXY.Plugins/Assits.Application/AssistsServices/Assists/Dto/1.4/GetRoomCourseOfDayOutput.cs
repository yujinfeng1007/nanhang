namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.4.获取教室当天课程表
    /// </summary>
    public class GetRoomCourseOfDayOutput
    {
        /// <summary>
        /// 课程Id
        /// </summary>
        public string F_CourseId { get; set; }

        /// <summary>
        /// 课程名称
        /// </summary>
        public string F_CourseName { get; set; }

        /// <summary>
        /// 第几个课
        /// </summary>
        public string F_LessonNo { get; set; }

        /// <summary>
        /// 时间段
        /// </summary>
        public string F_TimeSelect { get; set; }
    }
}