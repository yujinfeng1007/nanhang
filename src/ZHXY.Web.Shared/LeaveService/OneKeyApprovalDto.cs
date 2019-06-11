namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 一键审批Dto
    /// </summary>
    public class OneKeyApprovalDto
    {
        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string CurrentUserId { get; set; }

        /// <summary>
        /// 请假Id
        /// </summary>
        public string[] Orders { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>
        public string Opinion { get; set; }

        /// <summary>
        /// 是否同意
        /// </summary>
        public bool IsAgreed { get; set; }
    }
}