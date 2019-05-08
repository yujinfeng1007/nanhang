using System;

namespace ZHXY.Application
{
    public class CreateOrganizeDto
    {
        /// <summary>
        /// 上级机构Id
        /// </summary>
        public string ParentId { get; set; }
        /// <summary>
        /// 机构类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 机构名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 机构编号
        /// </summary>
        public string OrgNumber { get; set; }
        /// <summary>
        /// 负责人
        /// </summary>
        public string   Head { get; set; }
        public string MobilePhone { get; set; }
        public string WeChat { get; set; }
        public string TelPhone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Address { get; set; }

    }

    public class OrgTree : TreeView
    {
        public string Category { get; set; }
        public DateTime CreateTime { get; set; }
        public string  Remark { get; set; }
    }

}