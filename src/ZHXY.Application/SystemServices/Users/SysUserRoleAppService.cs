using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 用户角色管理
    /// </summary>
    public class SysUserRoleAppService : AppService
    {
        public SysUserRoleAppService(IZhxyRepository r) : base(r)
        {
        }

        public SysUserRoleAppService()
        {
            R = new ZhxyRepository();
        }

        public List<UserRole> GetListByUserId(string userId) => Read<UserRole>(t => t.F_User == userId).ToList();

        public List<UserRole> GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var expression = ExtLinq.True<UserRole>();
            if (!string.IsNullOrEmpty(keyword))
            {
                expression = expression.And(t => t.F_User == keyword);
            }
            if (!string.IsNullOrEmpty(F_DepartmentId))
            {
                expression = expression.And(t => t.F_DepartmentId.Equals(F_DepartmentId));
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Start))
            {
                var CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
                expression = expression.And(t => t.F_CreatorTime >= CreatorTime_Start);
            }

            if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
            {
                var CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
                expression = expression.And(t => t.F_CreatorTime <= CreatorTime_Stop);
            }

            return Read(expression).Paging( pagination).ToList();
        }

        //用作导出


        public UserRole GetForm(string id) => Get<UserRole>(id);
        public void Submit(UserRole entity, string keyValue) => throw new NotImplementedException();
        public void Delete(string keyValue) => throw new NotImplementedException();
    }
}