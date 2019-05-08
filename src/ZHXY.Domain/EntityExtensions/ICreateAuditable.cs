using System;

namespace ZHXY.Domain
{
    /// <summary>
    ///     实体创建审计接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface ICreateAuditable : IEntity

    {
        string Id { get; set; }
        string CreatedByUserId { get; set; }
        string OwnerDeptId { get; set; }
        DateTime CreatedTime { get; set; }
    }
}