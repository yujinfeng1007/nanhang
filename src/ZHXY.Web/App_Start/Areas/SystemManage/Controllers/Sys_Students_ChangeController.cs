using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class Sys_Students_ChangeController : ControllerBase
    {
        private Sys_Students_Change_App app = new Sys_Students_Change_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            //将用户id替换成姓名
            // var creator = new object();
            // var modifier = new object();
            // Dictionary<string, object>  dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(Cons.USERS);
            //if (data.F_CreatorUserId != null && dic.TryGetValue(data.F_CreatorUserId,out creator)) {
            //     data.F_CreatorUserId = creator.GetType().GetProperty("fullname").GetValue(creator, null).ToString();
            // }
            // if (data.F_LastModifyUserId != null &&　dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            // {
            //     data.F_LastModifyUserId = modifier.GetType().GetProperty("fullname").GetValue(modifier, null).ToString();
            // }
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysStudentsChange entity, string keyValue)
        {
            entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            app.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        ////导出excel
        // [HttpGet] [HandlerAuthorize] public FileResult export(string keyword) { //参数
        // 字段名->string[]{"F_Id",value} IDictionary<string, string> parms = new Dictionary<string,
        // string>(); //过滤条件 if(!Ext.IsEmpty(keyword)) parms.Add("F_RealName", keyword);

        // DbParameter[] dbParameter = createParms(parms);

        // string exportSql = createExportSql("Sys_Students_Change", parms); //string exportSql = "";
        // //Console.WriteLine("exportSql==>" + exportSql); DataTable users =
        // app.getDataTable(exportSql, dbParameter); ///////////////////写流 MemoryStream ms = new
        // NPOIExcel().ToExcelStream(users, "学生档案异动记录列表"); ms.Seek(0, SeekOrigin.Begin); string
        // filename = "学生档案异动记录列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls"; return
        // File(ms, "application/ms-excel", filename); }

        // //导入excel [HttpPost] [HandlerAjaxOnly] [HandlerAuthorize] [ValidateAntiForgeryToken]
        // public ActionResult import(string filePath) { //////////////////处理数据(机构 岗位
        // 等字典替换，过滤不要的字段，修改表头) List< Sys_Students_Change_Entity> list = ExcelToList<
        // Sys_Students_Change_Entity>(Server.MapPath(filePath), "Sys_Students_Change");

        // ///////////////////入库 if (list == null) return Error("导入失败"); app.import(list); return
        // Success("导入成功。"); }
    }
}