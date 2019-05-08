using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 请假审批
    /// </summary>
    public class LeaveApprove : IEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 请假单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 审批人
        /// </summary>
        public virtual User Approver { get; set; }
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
        public int Result { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Opinion { get; set; }
    }
}