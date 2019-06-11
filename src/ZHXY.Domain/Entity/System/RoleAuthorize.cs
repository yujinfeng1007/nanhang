using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 角色授权
    /// </summary>
    public class RoleAuthorize :IEntity
    {
        public string Id { get; set; }
        public int? ItemType { get; set; }
        public string ItemId { get; set; }
        public int? ObjectType { get; set; }
        public string ObjectId { get; set; }
    }
}