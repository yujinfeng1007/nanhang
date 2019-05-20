using System;


namespace ZHXY.Common
{
    public class CurrentUser
    {
        /// <summary>
        /// 当前操作用户的Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        ///<summary>
        /// 部门Id
        /// </summary>
        public string OrganId { get; set; }
        /// <summary>
        /// 登录地址
        /// </summary>
        public string Ip { get; set; }
        /// <summary>
        /// 登录地址名称
        /// </summary>
        public string IpLocation { get; set; }
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
        public string DutyId { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 用户设置
        /// </summary>
        public string SetUp { get; set; }

        /// <summary>
        /// 用户角色信息
        /// </summary>
        public string[] Roles { get; set; }


        /// <summary>
        /// 班级信息
        /// </summary>
        public string Classes { get; set; }

    }
}