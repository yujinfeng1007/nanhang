using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 角色
    /// </summary>
    public class SysRole : IEntity
    {
        public string F_Id { get; set; }
        //public string F_OrganizeId { get; set; }
        ///// <summary>
        ///// 1角色  2岗位
        ///// </summary>
        //public int? F_Category { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        //public string F_Type { get; set; }

        //数据类型
        //public string F_Duty_type { get; set; }

        public string F_Data_Type { get; set; }

        //自定义数据权限机构字段
        public string F_Data_Deps { get; set; }

        //public bool? F_AllowEdit { get; set; }
        //public bool? F_AllowDelete { get; set; }
        public int? F_SortCode { get; set; }
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