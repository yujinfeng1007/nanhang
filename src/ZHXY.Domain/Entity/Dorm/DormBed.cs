namespace ZHXY.Domain
{
    /// <summary>
    /// 宿舍床位
    /// </summary>
    public class DormBed : Entity
    {
              
        /// <summary>
        /// 床位编号
        /// </summary>
        public string 床位编号{ get; set; }
        
              
        /// <summary>
        /// 校区
        /// </summary>
        public string F_Area{ get; set; }
        
              
        /// <summary>
        /// 楼编号
        /// </summary>
        public string F_Building_No{ get; set; }
        
              
        /// <summary>
        /// 楼层号
        /// </summary>
        public string F_Floor_No{ get; set; }
        
              
        /// <summary>
        /// 宿舍类型（单人间、多人间、带卫生间）
        /// </summary>
        public string F_Dorm_Type{ get; set; }
        
              
        /// <summary>
        /// 教室编号
        /// </summary>
        public string F_Dorm_No{ get; set; }
        
              
        /// <summary>
        /// 床位类型（上铺、下铺、单人床）
        /// </summary>
        public string F_Bed_Type{ get; set; }
        
              
        /// <summary>
        /// 状态（正常使用、检修中、已停用）
        /// </summary>
        public string F_Classroom_Status{ get; set; }
        
        
    }
}
