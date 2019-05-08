namespace ZHXY.Application
{
    public class UserListView
    {
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
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 用户类型
        /// </summary>
        public string UserType { get; set; }
        /// <summary>
        /// 用户身份
        /// </summary>
        public string UserIdentity { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 岗位名称
        /// </summary>
        public string DutyName { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreatedTime { get; set; }
        /// <summary>
        /// 是否允许登录
        /// </summary>
        public string AllowedToLogin { get; set; }
    }

}