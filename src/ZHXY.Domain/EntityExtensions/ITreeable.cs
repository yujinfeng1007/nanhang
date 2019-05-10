using System.Collections.Generic;
namespace ZHXY.Domain
{
    /// <summary>
    ///     实体可树化接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface ITreeable : IEntity
    {
        string Id { get; set; }
        string ParentId { get; set; }
    }
}