using System;
using ZHXY.Common;

namespace ZHXY.Application
{


    /// <summary>
    /// 获取头像审批列表
    /// </summary>
    public class GetFaceApprovalListDto: Pagination    
    {

        public string Keyword { get; set; }
        /// <summary>
        /// 当前用户的Id
        /// </summary>
        public string CurrentUserId { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        //public DateTime StartTime { get; set; }

        ///// <summary>
        ///// 截至时间
        ///// </summary>
        //public DateTime EndTime { get; set; }

        /// <summary>
        /// 搜索模式 0:未审批  1:已审批 
        /// </summary>
        public string SearchPattern { get; set; }

    }
}