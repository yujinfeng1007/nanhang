using System;
namespace ZHXY.Domain
{
    /// <summary>
    ///     实体删除审计接口
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public interface IDeleteAuditable : IEntity
    {
        string Id { get; set; }

        /// <summary>
        ///     逻辑删除标记
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        ///     删除实体的用户
        /// </summary>
        string DeletedByUserId { get; set; }

        /// <summary>
        ///     删除实体时间
        /// </summary>
        DateTime? DeletedTime { get; set; }
    }
}