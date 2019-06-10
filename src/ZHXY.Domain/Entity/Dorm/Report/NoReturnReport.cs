using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 未归报表
    /// </summary>
    public class NoReturnReport : IEntity
    {
        public string Id { get; set; }
        /// <summary>
        /// 学号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 院校
        /// </summary>
        public string College { get; set; }
        /// <summary>
        /// 班级
        /// </summary>
        public string ClassId { get; set; }
        /// <summary>
        /// 宿舍号
        /// </summary>
        public string DormId { get; set; }
        /// <summary>
        /// 学生
        /// </summary>
        public string StudentId { get; set; }
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 未归天数
        /// </summary>
        public decimal DayCount { get; set; }
        public DateTime? OutTime { get; set; }


        public virtual Org Organ { get; set; }
        public virtual DormRoom Dorm { get; set; }
       
    }
}
