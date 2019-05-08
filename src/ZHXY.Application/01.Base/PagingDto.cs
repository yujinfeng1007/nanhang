namespace ZHXY.Application
{
    public class PagingDto
    {
        /// <summary>
        /// 需要的页数
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int Rows { get; set; }

        /// <summary>
        /// 排序属性
        /// </summary>
        public string Sort
        {
            get
            {
                if (string.IsNullOrEmpty(Sidx)) return "false";
                return $"{Sidx} {Sord}";
            }
        }

        public string Sord { get; set; }
        public string Sidx { get; set; }

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public string Keyword { get; set; }


        public int Skip => Take * (Page==0?1:Page - 1);
        public int Take => Rows==0?10:Rows;
    }

    public static class PageExt
    {
        /// <summary>
        /// 计算总页数
        /// </summary>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns></returns>
        public static int ComputePageCount(this int recordCount, int pageSize) => recordCount % pageSize == 0 ? recordCount / pageSize : (recordCount / pageSize) + 1;

        /// <summary>
        /// 计算页数信息
        /// </summary>
        /// <param name="recordCount">总记录数</param>
        /// <param name="pageSize">分页大小</param>
        /// <returns>recordCount:总记录数 pageCount:总页数</returns>
        public static (int recordCount, int pageCount) ComputePage(this int recordCount, int pageSize) => (recordCount, recordCount % pageSize == 0 ? recordCount / pageSize : (recordCount / pageSize) + 1);
    }
}