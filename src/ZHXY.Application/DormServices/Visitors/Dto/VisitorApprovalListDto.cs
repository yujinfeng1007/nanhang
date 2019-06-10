namespace ZHXY.Application
{


    /// <summary>
    /// 访客审批列表
    /// </summary>
    public class VisitorApprovalListDto: Pagination    
    {

        public string Keyword { get; set; }
        /// <summary>
        /// 当前用户的Id
        /// </summary>
        public string CurrentUserId { get; set; }         
        /// <summary>
        /// 审批状态 0:未审批  1:已审批 
        /// </summary>
        public string ApprovalStatus { get; set; }

    }
}