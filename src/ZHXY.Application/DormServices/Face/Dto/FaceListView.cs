using System;
using ZHXY.Application;

namespace ZHXY.Application
{
    /// <summary>
    /// 头像审批列表Dto
    /// </summary>
    public class FaceListView
    {
        /// <summary>
        /// 头像审批Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 申请人名称
        /// </summary>
        public string ApplierName { get; set; }

        /// <summary>
        /// 审批前头像
        /// </summary>
        public string SubmitImg { get; set; }

        /// <summary>
        /// 提交后头像
        /// </summary>
        public string ApproveImg { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 审批时间
        /// </summary>
        public DateTime ApproveTime { get; set; }

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


    }

  
}