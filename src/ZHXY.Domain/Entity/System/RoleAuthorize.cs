using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 角色授权
    /// </summary>
    public class RoleAuthorize : EntityBase, ICreationAudited
    {
        public string F_Id { get; set; }
        public int? F_ItemType { get; set; }
        public string F_ItemId { get; set; }
        public int? F_ObjectType { get; set; }
        public string F_ObjectId { get; set; }
        public int? F_SortCode { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public bool? F_DeleteMark { get; set; }

        public string F_DepartmentId { get; set; }
    }
}