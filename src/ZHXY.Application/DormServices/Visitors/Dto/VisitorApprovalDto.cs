namespace ZHXY.Application
{
    /// <summary>
    /// 访客审批Dto
    /// </summary>
    public class VisitorApprovalDto
    {
       
        public string CurrentUserId { get; set; }
        /// <summary>
        /// 请假Id
        /// </summary>
        public string VisitId { get; set; }
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