using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

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



        public List<SysUserRole> GetListByUserId(string userId)
        {
           return Read<SysUserRole>(t => t.F_User == userId).ToList();
        }

        //public List<SysUserRole> GetList(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        //{
        //    var expression = ExtLinq.True<SysUserRole>();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        expression = expression.And(t => t.F_User == keyword);
        //    }
        //    if (!string.IsNullOrEmpty(F_DepartmentId))
        //    {
        //        expression = expression.And(t => t.F_DepartmentId.Equals(F_DepartmentId));
        //    }

        //    if (!string.IsNullOrEmpty(F_CreatorTime_Start))
        //    {
        //        var CreatorTime_Start = Convert.ToDateTime(F_CreatorTime_Start + " 00:00:00");
        //        expression = expression.And(t => t.F_CreatorTime >= CreatorTime_Start);
        //    }

        //    if (!string.IsNullOrEmpty(F_CreatorTime_Stop))
        //    {
        //        var CreatorTime_Stop = Convert.ToDateTime(F_CreatorTime_Stop + " 23:59:59");
        //        expression = expression.And(t => t.F_CreatorTime <= CreatorTime_Stop);
        //    }

        //    return Repository.FindList(expression, pagination);
        //}



        //public SysUserRole GetForm(string keyValue) => Read<>.Find(keyValue);


        //public void DeleteForm(string keyValue)
        //{
        //    var F_Id = keyValue.Split('|');
        //    var expression = ExtLinq.False<SysUserRole>();
        //    for (var i = 0; i < F_Id.Length - 1; i++)
        //    {
        //        var Id = F_Id[i];
        //        expression = expression.Or(t => t.F_Id == Id);
        //    }
        //    Repository.BatchDelete(expression);
        //}

        //public void SubmitForm(SysUserRole entity, string keyValue)
        //{
        //    if (!string.IsNullOrEmpty(keyValue))
        //    {
        //        entity.Modify(keyValue);
        //        Repository.Update(entity);
        //    }
        //    else
        //    {
        //        entity.Create();
        //        Repository.Insert(entity);
        //    }
        //}
    }
}