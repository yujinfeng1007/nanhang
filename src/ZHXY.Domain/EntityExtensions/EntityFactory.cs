using System;

namespace ZHXY.Domain
{
    /// <summary>
    ///     实体工厂
    ///     author: 余金锋
    ///     phone:  l33928l9OO7
    ///     email:  2965l9653@qq.com
    /// </summary>
    public static class EntityFactory
    {
        /// <summary>
        ///     创建实体
        /// </summary>
        public static T Make<T>() where T : BaseEntity, new() => new T
        {
            Id = Guid.NewGuid().ToString("N").ToUpper()
        };
    }
}