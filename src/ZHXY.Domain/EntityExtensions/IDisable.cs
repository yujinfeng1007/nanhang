namespace ZHXY.Domain
{
    /// <summary>
    ///     实体可禁用接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface IDisable : IModifiedAuditable
    {
        bool IsDisabled { get; set; }
    }
}