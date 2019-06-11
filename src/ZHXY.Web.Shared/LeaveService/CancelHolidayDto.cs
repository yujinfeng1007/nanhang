namespace ZHXY.Web.Shared
{
    public class CancelHolidayDto
    {
        /// <summary>
        /// 当前用户Id
        /// </summary>
        public string OperatorId { get; set; }

        /// <summary>
        /// 请假单Id
        /// </summary>
        public string OrderId { get; set; }

        /// <summary>
        /// 销假天数
        /// </summary>
        public decimal Days { get; set; }
    }
}