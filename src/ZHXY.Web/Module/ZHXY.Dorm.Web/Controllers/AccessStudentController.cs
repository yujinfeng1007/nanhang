using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace ZHXY.Dorm.Web.Controllers
{

    public class AccessStudentController : ZhxyWebControllerBase
    {
       

        private AccessStudentAppService AccessStudentApp { get; }
        private DormStudentAppService DormStudentApp { get; }

        public AccessStudentController(AccessStudentAppService accessStudentApp, DormStudentAppService dormStudentApp)
        {
            AccessStudentApp = accessStudentApp;
            DormStudentApp = dormStudentApp;
        }


        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword,string F_DevName)
        {
            var data = new
            {
                rows = AccessStudentApp.GetList(pagination,keyword, F_DevName).Select(
                    t =>
                    {
                        var dormInfo= DormStudentApp.Read<DormStudent>(p=>p.F_Student_ID==t.F_UserId).FirstOrDefault()?.DormInfo;
                        return new
                        {
                            t.F_Id,
                            t.F_DeviceId,
                            t.F_UserId,
                            F_DevName = t.Device?.Name,
                            F_StudentNum = t.F_UserNum,
                            F_StudentName = t.F_UserName
                            ,
                            F_Divis_ID = t.Student.F_Divis_ID,
                            F_Grade_ID = t.Student.F_Grade_ID,
                            F_Class_ID = t.Student.F_Class_ID,
                            F_StudentDormNum = dormInfo?.Title
                        };
                    }),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = AccessStudentApp.GetById(id);
            return Content(data.ToJson());
        }
         
        [HttpPost]
        public ActionResult SubmitForm(AccessStudent entity, string keyValue)
        {
            AccessStudentApp.AddAccessUser(entity.F_DeviceId,entity.F_UserId);
            return Message("操作成功");
        }

        [HttpPost]
        public ActionResult DeleteForm(string keyValue)
        {
            AccessStudentApp.Delete(keyValue);
            return Message("删除成功。");
        }

        //导出excel
        [HttpGet]
        public FileResult export(string keyword)
        {
            //参数 字段名->string[]{"F_Id",value}
            IDictionary<string, string> parms = new Dictionary<string, string>();
            //过滤条件
            if (!Ext.IsEmpty(keyword))
                parms.Add("F_RealName", keyword);

            DbParameter[] dbParameter = CreateParms(parms);

            string exportSql = CreateExportSql("AccessStudent", parms);
            //Console.WriteLine("exportSql==>" + exportSql);
            var users = AccessStudentApp.GetDataTable(exportSql, dbParameter);
            ///////////////////写流
            var ms = new NPOIExcel().ToExcelStream(users, "列表");
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "列表" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

    }
}