using System;

namespace ZHXY.Domain
{

    /// <summary>
    /// 销假
    /// </summary>
    public class CancelHoliday:IEntity
    {
        /// <summary>
        /// 请假单Id
        /// </summary>
        public string OrderId { get; set; }
        /// <summary>
        /// 销假人
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 销假时间
        /// </summary>
        public DateTime OperationTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 销假天数
        /// </summary>
        public decimal Days { get; set; }
    }
}


