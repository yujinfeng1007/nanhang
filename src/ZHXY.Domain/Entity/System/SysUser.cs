//using System;
//using System.ComponentModel.DataAnnotations;

//namespace ZHXY.Domain.Entity
//{
//    /// <summary>
//    /// 用户
//    /// </summary>
//    [Serializable]
//    public class SysUser : IEntity
//    {
//        /// <summary>
//        /// 用户主键
//        /// </summary>
//        [Display(Name = "用户主键")]
//        public string F_Id { get; set; }

//        /// <summary>
//        /// 账户
//        /// </summary>
//        [Display(Name = "账户")]
//        public string F_Account { get; set; }

//        /// <summary>
//        /// 姓名
//        /// </summary>
//        [Display(Name = "姓名")]
//        public string F_RealName { get; set; }

//        /// <summary>
//        /// 昵称
//        /// </summary>
//        [Display(Name = "昵称")]
//        public string F_NickName { get; set; }

//        /// <summary>
//        /// 头像
//        /// </summary>
//        [Display(Name = "头像")]
//        public string F_HeadIcon { get; set; }

//        /// <summary>
//        /// 性别
//        /// </summary>
//        [Display(Name = "性别")]
//        public bool? F_Gender { get; set; }

//        /// <summary>
//        /// 生日
//        /// </summary>
//        [Display(Name = "生日")]
//        public DateTime? F_Birthday { get; set; }

//        /// <summary>
//        /// 手机
//        /// </summary>
//        [Display(Name = "手机")]
//        public string F_MobilePhone { get; set; }

//        /// <summary>
//        /// 邮箱
//        /// </summary>
//        [Display(Name = "邮箱")]
//        public string F_Email { get; set; }

//        /// <summary>
//        /// 邮箱密码
//        /// </summary>
//        [Display(Name = "邮箱密码")]
//        public string EmailPassword { get; set; }

//        /// <summary>
//        /// 微信
//        /// </summary>
//        [Display(Name = "微信")]
//        public string F_WeChat { get; set; }

//        /// <summary>
//        /// 主管主键
//        /// </summary>
//        [Display(Name = "主管主键")]
//        public string F_ManagerId { get; set; }

//        /// <summary>
//        /// 安全级别
//        /// </summary>
//        [Display(Name = "安全级别")]
//        public int? F_SecurityLevel { get; set; }

//        /// <summary>
//        /// 所属部门
//        /// </summary>
//        [Display(Name = "所属部门")]
//        public string F_DepartmentId { get; set; }

//        /// <summary>
//        /// 角色主键
//        /// </summary>
//        [Display(Name = "用户主键")]
//        public string F_RoleId { get; set; }

//        /// <summary>
//        /// 岗位主键
//        /// </summary>
//        [Display(Name = "岗位主键")]
//        public string F_DutyId { get; set; }

//        /// <summary>
//        /// 是否管理员
//        /// </summary>
//        [Display(Name = "是否管理员")]
//        public bool? F_IsAdministrator { get; set; }

//        public string F_Signature { get; set; }
//        public string F_OrganizeId { get; set; }
//        public int? F_SortCode { get; set; }
//        public bool? F_DeleteMark { get; set; }
//        public bool? F_EnabledMark { get; set; }
//        public string F_Description { get; set; }
//        public DateTime? F_CreatorTime { get; set; }
//        public string F_CreatorUserId { get; set; }
//        public DateTime? F_LastModifyTime { get; set; }
//        public string F_LastModifyUserId { get; set; }
//        public DateTime? F_DeleteTime { get; set; }
//        public string F_DeleteUserId { get; set; }

//        //数据类型
//        public string F_Data_Type { get; set; }

//        //自定义数据权限机构字段
//        public string F_Data_Deps { get; set; }

//        //客户身份 字典客户身份
//        public string F_KHSF { get; set; }

//        //用户设置
//        public string F_User_SetUp { get; set; }

//        //管理班级  班主任 副班主任
//        public string F_Class { get; set; }

//        public DateTime? F_UpdateTime { get; set; }

//        public string F_File { get; set; }
//    }
//}