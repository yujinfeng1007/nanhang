using System;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 访客列表Dto
    /// </summary>
    public class VisitorListView
    {
        /// <summary>
        /// 访客审批Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 申请人名称
        /// </summary>
        public string ApplicantName { get; set; }
        /// <summary>
        /// 访客性别
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
        /// 提交时间
        /// </summary>
        public DateTime CreatedTime { get; set; }
        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApprovedTime { get; set; }
        /// <summary>
        /// 审批状态
        /// </summary>
        public string ApprovalStatus { get; set; }
        /// <summary>
        /// 审批结果  0:未审批  1:同意  -1:拒绝
        /// </summary>
        public string Result { get; set; }
        /// <summary>
        /// 审批意见
        /// </summary>
        public string Opinion { get; set; }

        public string ImgUrl { get; set; }

    }

  
}