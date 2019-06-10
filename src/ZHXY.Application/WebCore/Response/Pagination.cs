namespace ZHXY.Application
{
    /// <summary>
    ///     分页信息
    /// </summary>
    public class Pagination
    {
        /// <summary>
        ///     每页行数
        /// </summary>
        public int Rows { get; set; } = 10;

        /// <summary>
        ///     当前页
        /// </summary>
        public int Page { get; set; } = 1;

        /// <summary>
        ///     排序列
        /// </summary>
        public string Sidx { get; set; }

        /// <summary>
        ///     排序方向
        /// </summary>
        public string Sord { get; set; }

        /// <summary>
        ///     总记录数
        /// </summary>
        public int Records { get; set; }

        /// <summary>
        ///     总页数
        /// </summary>
        public int Total => Records > 0 ? Records % Rows == 0 ? Records / Rows : (Records / Rows) + 1 : 0;

        public int Skip => (Page - 1) * Rows;

    }
}