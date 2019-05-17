using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 访问申请
    /// </summary>
    public class VisitApply : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 申请时间
        /// </summary>
        public DateTime ApplicationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime? ProcessingTime { get; set; }
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

        public DateTime VisitStartTime { get; set; }

        public DateTime VisitEndOfTime { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }

        /// <summary>
        /// 审批状态  0:未审批  1:通过   -1:不通过
        /// </summary>
        public int Status { get; set; }


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
