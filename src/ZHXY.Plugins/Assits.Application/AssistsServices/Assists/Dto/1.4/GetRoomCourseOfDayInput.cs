using ZHXY.Application;
namespace ZHXY.Assists.Application
{
    /// <summary>
    /// 1.4.获取教室当天课程表
    /// </summary>
    public class GetRoomCourseOfDayInput : BaseApiInput
    {
        /// <summary>
        /// 时间YYYY-MM-DD
        /// </summary>
        public string F_Date { get; set; }
    }
}