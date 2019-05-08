using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 宿舍维修
    /// </summary>
    public class DormRepair : Entity
    {
        
              
        /// <summary>
        /// 工单号
        /// </summary>
        public string F_No{ get; set; }
        
              
        /// <summary>
        /// 报修人
        /// </summary>
        public string F_Subor{ get; set; }
        
              
        /// <summary>
        /// 信息来源
        /// </summary>
        public string F_Source{ get; set; }
        
              
        /// <summary>
        /// 故障设备
        /// </summary>
        public string F_Device{ get; set; }
        
              
        /// <summary>
        /// 故障类别1
        /// </summary>
        public string F_Type1{ get; set; }
        
              
        /// <summary>
        /// 故障类别2
        /// </summary>
        public string F_Type2{ get; set; }
        
              
        /// <summary>
        /// 故障描述
        /// </summary>
        public string F_Broke_Memo{ get; set; }
        
              
        /// <summary>
        /// 故障地点
        /// </summary>
        public string F_Broke_Address{ get; set; }
        
              
        /// <summary>
        /// 受理人
        /// </summary>
        public string F_Handler{ get; set; }
        
              
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? F_Handler_Date{ get; set; }
        
              
        /// <summary>
        /// 处理过程
        /// </summary>
        public string F_Handler_Process{ get; set; }
        
              
        /// <summary>
        /// 处理结果
        /// </summary>
        public string F_Handler_Result{ get; set; }
        
              
        /// <summary>
        /// ID
        /// </summary>
        
              
        /// <summary>
        /// 
        /// </summary>
        public string F_SuborName{ get; set; }
        
              
        /// <summary>
        /// 
        /// </summary>
        public string F_HandlerName{ get; set; }
        
              
        /// <summary>
        /// 
        /// </summary>
        public string F_DeviceNum{ get; set; }
        
                
        
    }
}
