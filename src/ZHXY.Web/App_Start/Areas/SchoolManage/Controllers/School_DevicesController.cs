/*******************************************************************************
 * Author: mario
 * Description: School_Devices  Controller类
********************************************************************************/

using NFine.Code;
using NFine.Code.Excel;
using NFine.Domain.Entity.SchoolManage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    //设备管理
    public class School_DevicesController : ControllerBase
    {
        private School_Devices_App app = new School_Devices_App();
        private ICache cache = CacheFactory.Cache();

        //开关机
        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult kgjs()
        {
            return View();
        }

        [HttpGet]
        [HandlerAuthorize]
        public virtual ActionResult kgj()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult kgjSet(Devices entity, string keyValue)
        {
            app.kgjSet(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult clearKgj(string keyValue)
        {
            app.clearKgj(keyValue);
            return Success("清除成功！");
        }

        //宣传模式
        public virtual ActionResult xc()
        {
            return View();
        }

        public virtual ActionResult xcs()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult xcSet(Devices entity, string keyValue)
        {
            app.xcSet(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult clearXc(string keyValue)
        {
            app.clearXc(keyValue);
            return Success("清除成功！");
        }

        //倒计时
        public virtual ActionResult djs()
        {
            return View();
        }

        public virtual ActionResult djss()
        {
            return View();
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult djsSet(Devices entity, string keyValue)
        {
            app.djsSet(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult clearDjs(string keyValue)
        {
            app.clearDjs(keyValue);
            return Success("清除成功！");
        }

        //清除班牌教室信息
        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        public ActionResult clearRoom(string keyValue, string room, string F_Sn)
        {
            app.clearRoom(keyValue, room, F_Sn);
            CacheFactory.Cache().RemoveCache(Cons.DEVICES);
            return Success("清除成功！");
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_Display_Style, string F_Device_Status)
        {
            var data = new
            {
                rows = app.GetList(pagination, keyword, F_Display_Style, F_Device_Status),
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

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormF_Sn(string F_Sn)
        {
            var data = app.GetFormByF_Sn(F_Sn);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSkinList()
        {
            WebHelper webhelp = new WebHelper();
            //string Url = "http://localhost:8096/Api/School_Skin/getSkinList";
            //string Url = "http://localhost:8082/Api/School_Skin/getSkinList";
            string Url = "" + Configs.GetValue("port") + "/Api/School_Skin/getSkinList";
            bool isPost = false;
            var SkinList = webhelp.HttpWebRequest(Url, isPost);
            return Content(SkinList);
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Devices entity, string keyValue, string F_Classroom_Id)
        {
            app.SubmitForm(entity, keyValue, F_Classroom_Id);
            cache.RemoveCache(Cons.DEVICES);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            app.Delete(keyValue);
            cache.RemoveCache(Cons.DEVICES);
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

            string exportSql = CreateExportSql("School_Devices", parms);
            //string exportSql = "";
            //Console.WriteLine("exportSql==>" + exportSql);
            DataTable dt = app.getDataTable(exportSql, dbParameter);
            ///////////////////写流
            MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "School_Devices列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "School_Devices列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
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
            //rules.Add("F_Device_Code", new string[] { "设备编号", "" });
            //rules.Add("F_Device_Name", new string[] { "设备名称", "" });
            //rules.Add("F_Brand", new string[] { "品牌", "" });
            //rules.Add("F_Type", new string[] { "类型", "" });
            //rules.Add("F_Size", new string[] { "尺寸", "" });
            //rules.Add("F_Device_Status", new string[] { "状态（运行中、已关机、检修、停用）", "" });
            //rules.Add("F_IP", new string[] { "设备IP", "" });
            //rules.Add("F_Sn", new string[] { "设备序列号", "" });
            //rules.Add("F_Classroom", new string[] { "教室ID", "" });
            //rules.Add("F_Classroom_Info", new string[] { "教室地址", "" });
            //rules.Add("F_IfCountdown", new string[] { "是否启动倒计时", "" });
            //rules.Add("F_Countdown_Title", new string[] { "倒计时标题", "" });
            //rules.Add("F_Countdown_StartTime", new string[] { "倒计时开始时间", "" });
            //rules.Add("F_Countdown_EndTime", new string[] { "倒计时结束时间", "" });
            //rules.Add("F_Display_Style", new string[] { "显示模式（横屏/竖屏）", "" });
            //rules.Add("F_Style", new string[] { "风格模式", "" });
            //rules.Add("F_Address", new string[] { "安装地点", "" });
            //rules.Add("F_Class", new string[] { "班级id", "" });
            //rules.Add("F_Class_Info", new string[] { "班级信息", "" });
            //rules.Add("F_Switch_Rules", new string[] { "定时开关机规则（周一、周二。。。）", "" });
            //rules.Add("F_HomePage_Channel", new string[] { "首页栏目（风采图片 视频 新闻 活动）", "" });
            //rules.Add("F_SystemNo", new string[] { "系统版本号", "" });
            //rules.Add("F_ApkNo", new string[] { "apk版本号", "" });

            //////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头
            List<Devices> list = ExcelToList<Devices>(Server.MapPath(filePath), rules);

            ///////////////////入库
            if (list == null)
                return Error("导入失败");
            app.import(list);
            return Success("导入成功。");
        }
    }
}