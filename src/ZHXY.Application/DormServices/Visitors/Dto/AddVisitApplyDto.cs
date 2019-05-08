using System;

namespace ZHXY.Application
{
    /// <summary>
    /// 访客申请Dto
    /// </summary>
    public class AddVisitApplyDto
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

        public DateTime VisitStartTime { get; set; }

        public DateTime VisitEndOfTime { get; set; }

        /// <summary>
        /// 关系
        /// </summary>
        public string Relationship { get; set; }


    }

}

