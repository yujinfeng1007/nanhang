using System;

namespace ZHXY.Domain.Entity
{
    /// <summary>
    /// 字典
    /// </summary>
    public class Dic : IEntity
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

    }
}