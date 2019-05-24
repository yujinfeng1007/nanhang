using System;

namespace ZHXY.Domain
{
    /// <summary>
    /// 用户角色关联
    /// </summary>
    public class SysUserRole :IEntity
    {
        public string F_Id { get; set; } = Guid.NewGuid().ToString("N").ToUpper();
        /// <summary>
        /// 用户Id
        /// </summary>

        public string F_User { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>

        public string F_Role { get; set; }



        /// <summary>
        /// 序号
        /// </summary>

        public int? F_SortCode { get; set; }

    }
}