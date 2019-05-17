using System.ComponentModel.DataAnnotations;

namespace ZHXY.Application
{
    /// <summary>
    /// 头像申请Dto
    /// </summary>
    public class FaceRequestDto
    {
        /// <summary>
        /// 头像申请Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 申请人名称
        /// </summary>
        //[Required]
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
        /// 审批人
        /// </summary>
        public string[] Approvers { get; set; }

    }
}