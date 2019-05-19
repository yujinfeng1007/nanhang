using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 长时间未出报表
    /// </summary>
    public class NoOutReport: IEntity
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
        /// 未出时长
        /// </summary>
        public decimal Time { get; set; }

        /// <summary>
        /// 进宿舍时间
        /// </summary>
        public DateTime? InTime { get; set; }

       

        public virtual Organ Organ { get; set; }
        public virtual DormRoom Dorm{ get; set; }
        
    }
}
