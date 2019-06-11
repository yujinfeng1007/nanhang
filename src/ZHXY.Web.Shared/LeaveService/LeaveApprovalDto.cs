namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 请假审批Dto
    /// </summary>
    public class LeaveApprovalDto
    {
       
        public string CurrentUserId { get; set; }
        /// <summary>
        /// 请假Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 是否同意
        /// </summary>
        public bool IsAgreed { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Opinion { get; set; }

    
        
    }
}