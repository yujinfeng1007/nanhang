using System;

namespace ZHXY.Application
{
    public class OrganizeDto
    {
        public string F_Id { get; set; }
        public string F_ParentId { get; set; }
        public int? F_Layers { get; set; }
        public string F_EnCode { get; set; }
        public string F_FullName { get; set; }
        public string F_ShortName { get; set; }
        public string F_CategoryId { get; set; }

        public int? F_SortCode { get; set; }
        public bool? F_DeleteMark { get; set; }
        public bool? F_EnabledMark { get; set; }
        public string F_Description { get; set; }
        public DateTime? F_CreatorTime { get; set; }
        public string F_CreatorUserId { get; set; }
        public DateTime? F_LastModifyTime { get; set; }
        public string F_LastModifyUserId { get; set; }
        public DateTime? F_DeleteTime { get; set; }
        public string F_DeleteUserId { get; set; }
        //班级类型
        public string F_Class_Type { get; set; }
        //报名模版
        public string F_Template { get; set; }
        //年度
        public int? F_Year { get; set; }

        public string F_DepartmentId { get; set; }
    }
    }