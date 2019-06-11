namespace ZHXY.Domain
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    public class UserRole :IEntity
    {

        public string UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>

        public string RoleId { get; set; }

    }
}