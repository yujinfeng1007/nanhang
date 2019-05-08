/*******************************************************************************
 * Author: mario
 * Description: School_Schedules  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //班级课程信息表
    public class School_SchedulesController : ControllerBase
    {
        private School_Schedules_App app = new School_Schedules_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
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
        public ActionResult SubmitForm(Schedule entity, string keyValue)
        {
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

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("School_Schedules", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Schedules列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Schedules列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        //导入excel
        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult import(string filePath)
        {
            //////////////////定义规则：字段名，表头名称，字典
            //字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
            IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
            //rules.Add("F_Id", new string[] { "编号", "" });
            //rules.Add("F_RealName", new string[] { "姓名", "" });
            //rules.Add("F_Gender", new string[] { "性别", "104" });
            //rules.Add("F_OrganizeId", new string[] { "公司", "F_OrganizeId" });
            //rules.Add("F_DepartmentId", new string[] { "部门", "F_DepartmentId" });
            //rules.Add("F_AreaId", new string[] { "地区", "F_AreaId" });
            //rules.Add("F_RoleId", new string[] { "角色", "F_RoleId" });
            //rules.Add("F_DutyId", new string[] { "岗位", "F_DutyId" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_HeadIcon", new string[] { "头像", "" });

            //所有字段代码
            //rules.Add("F_Id", new string[] { "ID", "" });
            //rules.Add("F_SortCode", new string[] { "序号", "" });
            //rules.Add("F_DepartmentId", new string[] { "所属部门", "" });
            //rules.Add("F_DeleteMark", new string[] { "删除标记", "" });
            //rules.Add("F_EnabledMark", new string[] { "启用标记", "" });
            //rules.Add("F_CreatorTime", new string[] { "创建时间", "" });
            //rules.Add("F_CreatorUserId", new string[] { "创建者", "" });
            //rules.Add("F_LastModifyTime", new string[] { "修改时间", "" });
            //rules.Add("F_LastModifyUserId", new string[] { "修改者", "" });
            //rules.Add("F_DeleteTime", new string[] { "删除时间", "" });
            //rules.Add("F_DeleteUserId", new string[] { "删除者", "" });
            //rules.Add("F_Memo", new string[] { "备注", "" });
            //rules.Add("F_Class", new string[] { "班级ID", "" });
            //rules.Add("F_Classroom", new string[] { "教室ID", "" });
            //rules.Add("F_Date", new string[] { "日期", "" });
            //rules.Add("F_Week", new string[] { "星期", "" });
            //rules.Add("F_Course1", new string[] { "科目", "" });
            //rules.Add("F_Teacher1", new string[] { "授课老师", "" });
            //rules.Add("F_Check1", new string[] { "签到", "" });
            //rules.Add("F_Course_PrepareID1", new string[] { "备课ID", "" });
            //rules.Add("F_Course2", new string[] { "科目2", "" });
            //rules.Add("F_Teacher2", new string[] { "授课老师2", "" });
            //rules.Add("F_Course_PrepareID2", new string[] { "备课ID2", "" });
            //rules.Add("F_Check2", new string[] { "签到2", "" });
            //rules.Add("F_Course3", new string[] { "科目3", "" });
            //rules.Add("F_Teacher3", new string[] { "授课老师3", "" });
            //rules.Add("F_Course_PrepareID3", new string[] { "备课ID3", "" });
            //rules.Add("F_Check3", new string[] { "签到", "" });
            //rules.Add("F_Course4", new string[] { "科目4", "" });
            //rules.Add("F_Teacher4", new string[] { "授课老师4", "" });
            //rules.Add("F_Course_PrepareID4", new string[] { "备课ID4", "" });
            //rules.Add("F_Check4", new string[] { "签到2", "" });
            //rules.Add("F_Course5", new string[] { "科目5", "" });
            //rules.Add("F_Teacher5", new string[] { "授课老师5", "" });
            //rules.Add("F_Course_PrepareID5", new string[] { "备课ID5", "" });
            //rules.Add("F_Check5", new string[] { "签到2", "" });
            //rules.Add("F_Course6", new string[] { "科目6", "" });
            //rules.Add("F_Teacher6", new string[] { "授课老师6", "" });
            //rules.Add("F_Course_PrepareID6", new string[] { "备课ID6", "" });
            //rules.Add("F_Check6", new string[] { "签到2", "" });
            //rules.Add("F_Course7", new string[] { "科目7", "" });
            //rules.Add("F_Teacher7", new string[] { "授课老师7", "" });
            //rules.Add("F_Course_PrepareID7", new string[] { "备课ID7", "" });
            //rules.Add("F_Check7", new string[] { "签到2", "" });
            //rules.Add("F_Course8", new string[] { "科目8", "" });
            //rules.Add("F_Teacher8", new string[] { "授课老师8", "" });
            //rules.Add("F_Course_PrepareID8", new string[] { "备课ID8", "" });
            //rules.Add("F_Check8", new string[] { "签到2", "" });
            //rules.Add("F_Course9", new string[] { "科目9", "" });
            //rules.Add("F_Teacher9", new string[] { "授课老师9", "" });
            //rules.Add("F_Course_PrepareID9", new string[] { "备课ID9", "" });
            //rules.Add("F_Check9", new string[] { "签到2", "" });
            //rules.Add("F_Course10", new string[] { "科目10", "" });
            //rules.Add("F_Teacher10", new string[] { "授课老师10", "" });
            //rules.Add("F_Course_PrepareID10", new string[] { "备课ID10", "" });
            //rules.Add("F_Check10", new string[] { "签到2", "" });
            //rules.Add("F_Course11", new string[] { "科目11", "" });
            //rules.Add("F_Teacher11", new string[] { "授课老师11", "" });
            //rules.Add("F_Course_PrepareID11", new string[] { "备课ID11", "" });
            //rules.Add("F_Check11", new string[] { "签到2", "" });
            //rules.Add("F_Course12", new string[] { "科目12", "" });
            //rules.Add("F_Teacher12", new string[] { "授课老师12", "" });
            //rules.Add("F_Course_PrepareID12", new string[] { "备课ID12", "" });
            //rules.Add("F_Check12", new string[] { "签到2", "" });
            //rules.Add("F_Semester", new string[] { "所属学期", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头
            List<Schedule> list = ExcelToList<Schedule>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }

        // 根据班级和日期，获取当前日期上的课程
        public ActionResult GetFormJsonByDay(string Day, string F_Class)
        {
            DateTime f_date = Convert.ToDateTime(Day);
            return Content(app.GetList(t => t.F_Class == F_Class && t.F_Date == f_date).FirstOrDefault().ToJson());
        }
    }
}