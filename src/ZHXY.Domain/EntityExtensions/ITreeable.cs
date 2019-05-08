using System.Collections.Generic;
namespace ZHXY.Domain
{
    /// <summary>
    ///     实体可树化接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface ITreeable<T> : ITreeable
    {
        T Parent { get; set; }
        ICollection<T> Children { get; set; }
    }

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