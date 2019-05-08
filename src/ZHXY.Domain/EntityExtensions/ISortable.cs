namespace ZHXY.Domain
{
    /// <summary>
    ///     实体可排序接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface ISortable : IModifiedAuditable
    {
        int? SortCode { get; set; }
    }
}