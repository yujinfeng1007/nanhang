using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 宿舍
    /// </summary>
    public class DormRoom : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        /// <summary>
        /// 校区
        /// </summary>
        public string Area{ get; set; }


        /// <summary>
        /// 楼层号
        /// </summary>
        public string FloorNumber { get; set; }
        
              
        /// <summary>
        /// 楼栋Id
        /// </summary>
        public string BuildingId{ get; set; }
        
              
        /// <summary>
        /// 容纳人数
        /// </summary>
        public int? Capacity{ get; set; }
        
              
        /// <summary>
        /// 宿舍类型（单人间、多人间、带卫生间）
        /// </summary>
        public string DormType{ get; set; }
        
              
        /// <summary>
        /// 教室编号
        /// </summary>
        public string RoomNumber{ get; set; }
        
              
        /// <summary>
        /// 允许入住性别(男、女、不限)
        /// </summary>
        public string AllowGender{ get; set; }
        
              
        /// <summary>
        /// 已住人数
        /// </summary>
        public int? F_In{ get; set; }
        
              
        /// <summary>
        /// 未住人数
        /// </summary>
        public int? F_Free{ get; set; }
        
              
        /// <summary>
        /// 状态（已满、有床位、已停用）
        /// </summary>
        public string Status{ get; set; }
        
              
        /// <summary>
        /// 宿舍标题
        /// </summary>
        public string Title{ get; set; }
        
              
        /// <summary>
        /// 舍长ID
        /// </summary>
        public string ManagerId{ get; set; }

        /// <summary>
        /// 宿管ID
        /// </summary>
        public string AdminstratorId { get; set; }
              
        public virtual Building Building { get; set; }
    }
}
