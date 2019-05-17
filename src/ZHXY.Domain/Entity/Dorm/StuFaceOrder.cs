using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZHXY.Domain
{
    /// <summary>
    /// 头像申请
    /// </summary>
    public class StuFaceOrder : IEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();


        public DateTime CreatedTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 申请人id
        /// </summary>
        public string ApplicantId { get; set; }
       

        /// <summary>
        /// 审批前头像
        /// </summary>
        public string SubmitImg { get; set; }

        /// <summary>
        /// 提交后头像
        /// </summary>
        public string ApproveImg { get; set; }

        /// <summary>
        /// 状态(0:未审批 1:已审批)
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 审批意见
        /// </summary>

        public string ApprovalOpinion { get; set; }

        /// <summary>
        /// 宿管
        /// </summary>
        //public virtual User DormManager { get; set; }
        

        /// <summary>
        /// 申请人
        /// </summary>
        public virtual User Applicant { get; set; }
    }
}