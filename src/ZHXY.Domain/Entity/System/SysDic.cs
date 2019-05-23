using System;

namespace ZHXY.Domain.Entity
{
    /// <summary>
    /// 字典
    /// </summary>
    public class SysDic : IEntity
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        //public bool? F_IsTree { get; set; }
        //public int? F_Layers { get; set; }
        //public int? F_SortCode { get; set; }
        //public bool? F_DeleteMark { get; set; }
        //public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        //public string F_CreatorUserId { get; set; }
        //public DateTime? F_LastModifyTime { get; set; }
        //public string F_LastModifyUserId { get; set; }
        //public DateTime? F_DeleteTime { get; set; }
        //public string F_DeleteUserId { get; set; }

        //public string F_DepartmentId { get; set; }
    }
}