using System;
namespace ZHXY.Domain
{
    /// <summary>
    ///     实体修改审计接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface IModifiedAuditable : IEntity
    {
        string Id { get; set; }
        string LastModifiedByUserId { get; set; }
        DateTime? LastModifiedTime { get; set; }
    }
}