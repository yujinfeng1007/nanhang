namespace ZHXY.Domain
{
    public abstract class ReportEntity: Entity
    {
        /// <summary>
        /// 学号
        /// </summary>
        public string F_Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string F_Name { get; set; }
        /// <summary>
        /// 院校
        /// </summary>
        public string F_College { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string F_Class { get; set; }
        /// <summary>
        /// 宿舍号
        /// </summary>
        public string F_Dorm { get; set; }
        /// <summary>
        /// 学生
        /// </summary>
        public string F_StudentId { get; set; }

    }
}
