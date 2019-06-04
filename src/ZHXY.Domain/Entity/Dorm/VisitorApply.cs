using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 访问申请
    /// </summary>
    public class VisitorApply : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        /// <summary>
        /// 申请时间     
        /// </summary>
        public DateTime CreatedTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ApprovedTime { get; set; }
        /// <summary>
        /// 入住性别
        /// </summary>
        public string VisitorGender { get; set; }
        /// <summary>
        /// 访客姓名
        /// </summary>
        public string VisitorName { get; set; }
        /// <summary>
        /// 访客证件号
        /// </summary>
        public string VisitorIDCard { get; set; }
        /// <summary>
        /// 探访事由
        /// </summary>
        public string VisitReason { get; set; }
        /// <summary>
        /// 访客类型
        /// </summary>
        public string VisitType { get; set; }
        /// <summary>
        /// 申请人Id
        /// </summary>
        public string ApplicantId { get; set; }
        /// <summary>
        /// 宿舍ID
        /// </summary>
        public string DormId { get; set; }
        /// <summary>
        /// 楼房ID
        /// </summary>
        public string BuildingId { get; set; }
        /// <summary>
        /// 探访开始时间
        /// </summary>
        public DateTime VisitStartTime { get; set; }
        /// <summary>
        /// 探访结束时间
        /// </summary>
        public DateTime VisitEndTime { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }
        /// <summary>
        /// 审批状态(0:未审批 1:已审批)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 访客头像（校外访客）
        /// </summary>
        public string ImgUri { get; set; }

        public string DhId { get; set; }

        /// <summary>
        /// 学生基础信息
        /// </summary>
        public virtual Student Student {get;set;}
        /// <summary>
        /// 宿舍信息
        /// </summary>
        public virtual DormRoom DormRoom { get; set; }
                
        
    }
}
