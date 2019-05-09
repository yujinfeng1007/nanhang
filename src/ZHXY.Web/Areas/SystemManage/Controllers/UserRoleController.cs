using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleController : ZhxyWebControllerBase
    {
        private SysUserRoleAppService App { get; }

        public UserRoleController(SysUserRoleAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetForm(keyValue);
            //将用户id替换成姓名
            var creator = new User();
            var modifier = new User();
            var dic = CacheFactory.Cache().GetCache<Dictionary<string, User>>(SYS_CONSTS.USERS);
            if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId, out creator))
            {
                data.F_CreatorUserId = creator.F_RealName;
            }
            if (data.F_LastModifyUserId != null && dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            {
                data.F_LastModifyUserId = modifier.F_RealName;
            }
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(UserRole entity, string keyValue)
        {
            entity.F_DepartmentId = OperatorProvider.Current.DepartmentId;
            App.Submit(entity, keyValue);
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

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword)
        {
            var pagination = new Pagination();
            pagination.Sord = "desc";
            pagination.Sidx = "F_CreatorTime desc";
            pagination.Rows = 1000000;
            pagination.Page = 1;
            var list = App.GetList(pagination, keyword, string.Empty, string.Empty, string.Empty);
            var dt = ListToDataTable(list, new Dictionary<string, string[]>());
            var ms = new NPOIExcel().ToExcelStream(dt, "Sys_User_Role列表");
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "Sys_User_Role列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }
    }
}