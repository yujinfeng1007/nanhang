using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 宿舍学生
    /// </summary>
    public class DormStudent : Entity
    {
              
        /// <summary>
        /// 学生ID
        /// </summary>
        public string F_Student_ID{ get; set; }
        
              
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public string F_DormId{ get; set; }
        
              
        /// <summary>
        /// 床位ID
        /// </summary>
        public string F_Bed_ID{ get; set; }
        
              
        /// <summary>
        /// 入住时间
        /// </summary>
        public DateTime? F_In_Time{ get; set; }
        
              
              
        /// <summary>
        /// 入住性别
        /// </summary>
        public string F_Sex{ get; set; }
        public virtual DormRoom DormInfo { get; set; }

        public virtual Student Student { get; set; }

    }
}
