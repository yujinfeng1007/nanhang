using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 访客申请Dto
    /// </summary>
    public class VisitorApplySubmitDto
    {
        public string VisitorGender { get; set; }
        /// <summary>
        /// 访客姓名
        /// </summary>
        public string VisitorName { get; set; }
        /// <summary>
        /// 访客证件号
        /// </summary>
        public string VisitorIDCard { get; set; }
        /// <summary>
        /// 探访事由
        /// </summary>
        public string VisitReason { get; set; }
        /// <summary>
        /// 访客类型
        /// </summary>
        public string VisitType { get; set; }
        /// <summary>
        /// 探访开始时间
        /// </summary>
        public DateTime VisitStartTime { get; set; }
        /// <summary>
        /// 探访结束时间
        /// </summary>
        public DateTime VisitEndTime { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }


    }

}

