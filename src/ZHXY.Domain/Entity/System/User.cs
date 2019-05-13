using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 用户
    /// </summary>
    [Serializable]
    public class User : IEntity
    {
        /// <summary>
        /// 用户主键
        /// </summary>
        public string Id { get; set; }

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
        public bool? Gender { get; set; }

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
        /// 微信
        /// </summary>
        public string WeChat { get; set; }

        /// <summary>
        /// 所属机构
        /// </summary>
        public string OrganId { get; set; }

        /// <summary>
        /// 角色主键
        /// </summary>
        public string RoleId { get; set; }

        /// <summary>
        /// 岗位主键
        /// </summary>
        public string DutyId { get; set; }

        public bool? DeleteMark { get; set; }
        public bool? EnabledMark { get; set; }
        public DateTime? CreatorTime { get; set; }

        public string DataType { get; set; }

        public string DataDeps { get; set; }


        public string UserSetUp { get; set; }

        public string Class { get; set; }

        public DateTime? UpdateTime { get; set; }

        public string File { get; set; }
    }
}