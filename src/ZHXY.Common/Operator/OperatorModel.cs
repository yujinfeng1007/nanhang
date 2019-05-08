using System;
using System.Collections.Generic;

namespace ZHXY.Common
{
    public class OperatorModel
    {
        /// <summary>
        /// 当前操作用户的Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户代码
        /// </summary>
        public string UserCode { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 用户学校Id
        /// </summary>
        public string CompanyId { get; set; }
        /// <summary>
        /// 学校名称
        /// </summary>
        public string SchoolName { get; set; }
        /// <summary>
        /// 学校代码
        /// </summary>
        public string SchoolCode { get; set; }
        /// <summary>
        /// 部门Id
        /// </summary>
        public string DepartmentId { get; set; }
        /// <summary>
        /// 角色Id
        /// </summary>
        public string RoleId { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string LoginIPAddress { get; set; }
        /// <summary>
        /// 登录地址名称
        /// </summary>
        public string LoginIPAddressName { get; set; }
        /// <summary>
        /// 登录标识
        /// </summary>
        public string LoginToken { get; set; }
        /// <summary>
        /// 登录时间
        /// </summary>
        public DateTime LoginTime { get; set; } = DateTime.Now;
        /// <summary>
        /// 是否是系统管理员
        /// </summary>
        public bool IsSystem { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadIcon { get; set; }

        /// <summary>
        /// 岗位 老师 学生 家长
        /// </summary>
        public string Duty { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 用户设置
        /// </summary>
        public string F_User_SetUp { get; set; }

        /// <summary>
        /// 用户班级信息
        /// </summary>
        public Dictionary<string, object> Classes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// 用户角色信息
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> Roles { get; set; } = new Dictionary<string, Dictionary<string, string>>();

    }
}