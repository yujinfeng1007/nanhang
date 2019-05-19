using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 宿舍学生
    /// </summary>
    public class DormStudent : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 学生ID
        /// </summary>
        public string StudentId{ get; set; }
        
              
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public string DormId{ get; set; }
        
              
        /// <summary>
        /// 床位ID
        /// </summary>
        public string BedId{ get; set; }
        
              
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime? InTime{ get; set; }
              
        /// <summary>
        /// 入住性别
        /// </summary>
        public string Gender{ get; set; }

        public string Description { get; set; }

        public virtual DormRoom DormInfo { get; set; }

        public virtual Student Student { get; set; }
       
    }
}
