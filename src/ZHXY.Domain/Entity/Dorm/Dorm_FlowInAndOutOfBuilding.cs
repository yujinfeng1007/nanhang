using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 流动进出流水
    /// </summary>
    public partial class LDJCLS:IEntity
    {
        /// <summary>
        /// 流水Id
        /// </summary>
        public string FlowId { get; set; }

        /// <summary>
        /// 楼栋号
        /// </summary>
        public string BuildingNo { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 进出方向
        /// </summary>
        public bool? Direction { get; set; }

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime? OccurrenceTime { get; set; }

        /// <summary>
        /// 是否异常
        /// </summary>
        public bool? IsAbnormal { get; set; }
    }
}
