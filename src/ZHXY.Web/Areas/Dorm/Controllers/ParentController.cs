using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 家长管理
    /// </summary>
    public class ParentController : ZhxyWebControllerBase
    {
        private SysUserAppService App { get; }

        public ParentController(SysUserAppService app) => App = app;

        [HttpGet]

        public ActionResult GetGridJson(Pagination pagination, string F_Account, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = App.GetParentsList(pagination, F_Account, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop);
            return PagingResult(data, pagination.Records, pagination.Total);

        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue) => Content(App.Get(keyValue).ToJson());

        [HttpPost]

        public ActionResult SubmitForm(UserDto userEntity, UserLoginDto userLogOnEntity, string keyValue)
        {
            userEntity.F_DepartmentId = "parent";
            userEntity.F_OrganizeId = "1";
            userEntity.F_DutyId = "parentDuty";
            userEntity.F_RoleId = "parent";
            userEntity.F_Account = userEntity.F_MobilePhone;
            userEntity.F_EnabledMark = true;
            App.ParentSubmit(userEntity, userLogOnEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult UpdataSubmitForm(UserDto userEntity, UserLoginDto userLogOnEntity, string keyValue)
        {
            userEntity.F_DepartmentId = "parent";
            userEntity.F_OrganizeId = "1";
            userEntity.F_DutyId = "parentDuty";
            userEntity.F_RoleId = "parent";
            userEntity.F_Account = userEntity.F_MobilePhone;
            userEntity.F_EnabledMark = true;
            App.Submit(userEntity, userLogOnEntity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
          
            return Message("删除成功。");
        }

        [HttpGet]
        public ActionResult RevisePassword() => View();

        [HttpPost]

        public ActionResult SubmitRevisePassword(string userPassword, string keyValue)
        {
            new SysUserLogOnAppService().RevisePassword(userPassword, keyValue);
            return Message("重置密码成功。");
        }



       

    }
}