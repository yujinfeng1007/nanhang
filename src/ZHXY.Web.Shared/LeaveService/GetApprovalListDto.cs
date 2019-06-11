using System;

namespace ZHXY.Web.Shared
{
    /// <summary>
    /// 获取审批列表
    /// </summary>
    public class GetApprovalListDto: Pagination
    {

        public string Keyword { get; set; }
        /// <summary>
        /// 当前用户的Id
        /// </summary>
        public string CurrentUserId { get; set; }
          /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// 截至时间
        /// </summary>
        public DateTime EndOfTime { get; set; }

        /// <summary>
        /// 搜索模式 0:未审批  1:已审批 
        /// </summary>
        public string Status { get; set; }

    }
}