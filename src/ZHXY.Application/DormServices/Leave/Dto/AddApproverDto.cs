using System.ComponentModel.DataAnnotations;

namespace ZHXY.Application
{
    /// <summary>
    /// 添加二级审批人Dto
    /// </summary>
    public class AddApproverDto
    {
        /// <summary>
        /// 请假单Id
        /// </summary>
        [Required]
        public string OrderId { get; set; }
        /// <summary>
        /// 审批人Id
        /// </summary>
        [Required]
        public string ApproverId { get; set; }
    }
}