namespace ZHXY.Web.Shared
{
    public class GetOriginalListByDateParms
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { get; set; } = 1;
        /// <summary>
        /// 页面行数
        /// </summary>
        public int PageSize { get; set; } = 15;
        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { get; set; }
    }
}