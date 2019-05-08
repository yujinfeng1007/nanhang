namespace ZHXY.Domain
{
    /// <summary>
    /// 学生假期设置
    /// </summary>
    public class StuHolidayLimit : IEntity
    {
        /// <summary>
        /// 学期Id
        /// </summary>
        public string SemesterId { get; set; }
        /// <summary>
        /// 学生Id
        /// </summary>
        public string StudentId { get; set; }
        /// <summary>
        /// 已使用天数
        /// </summary>
        public decimal UsedDays { get; set; }
    }
}


