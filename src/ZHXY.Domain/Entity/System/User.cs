using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : IEntity
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string HeadIcon { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public bool Gender { get; set; }

        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string MobilePhone { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }



        /// <summary>
        /// 所属机构
        /// </summary>
        public string OrganId { get; set; }
        /// <summary>
        /// 岗位主键
        /// </summary>
        public string DutyId { get; set; }

      

        public string SetUp { get; set; }

        public string Password { get; set; } = "17f123f731bca5b8925979dc1a228548";
        /// <summary>
        /// 安全码
        /// </summary>
        public string Secretkey { get; set; } = "4a7d1ed414474e40";
        /// <summary>
        /// 上次访问时间
        /// </summary>
        public DateTime? PreVisitTime { get; set; }
        /// <summary>
        /// 最后访问时间
        /// </summary>
        public DateTime? LastVisitTime { get; set; }
        /// <summary>
        /// 登录总次数
        /// </summary>
        public int LoginCount { get; set; }

    }
}