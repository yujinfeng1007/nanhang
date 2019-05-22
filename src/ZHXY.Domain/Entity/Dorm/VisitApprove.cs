using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 头像审批
    /// </summary>
    public class VisitApprove : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 审批单Id
        /// </summary>
        public string VisitId { get; set; }
       
        /// <summary>
        /// 审批人Id
        /// </summary>
        public string ApproverId { get; set; }
        

        /// <summary>
        /// 审批级别
        /// </summary>
        public int ApproveLevel { get; set; }

        /// <summary>
        /// 审批结果  0:未审批  1:同意  -1:拒绝
        /// </summary>
        public string ApproveResult { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public virtual User Approver { get; set; }

    }
}