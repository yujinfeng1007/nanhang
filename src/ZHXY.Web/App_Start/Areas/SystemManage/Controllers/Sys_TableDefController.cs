using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class Sys_TableDefController : ControllerBase
    {
        private Sys_TableDef_App app = new Sys_TableDef_App();
        private Sys_FieldDef_App fieldapp = new Sys_FieldDef_App();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string keyword)
        {
            var data = app.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = app.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysTableDef entity, string keyValue)
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
            fieldapp.DeleteByTableId(keyValue);
            return Success("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyValue)
        {
            /////////////////获得数据集合
            List<SysTableDef> list = app.GetList();
            return ToMuchSheetExcel(list);
        }

        public FileResult ToMuchSheetExcel(List<SysTableDef> list)
        {
            MemoryStream ms = new MemoryStream();
            IWorkbook workBook = new HSSFWorkbook();
            for (int i = 0; i < list.Count; i++)
            {
                IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
                rules.Add("F_FieldName", new string[] { "字段名称", string.Empty });
                rules.Add("F_FieldTitle", new string[] { "字段标题", string.Empty });
                rules.Add("F_DataType", new string[] { "数据类型", string.Empty });
                rules.Add("F_Length", new string[] { "数据长度", string.Empty });
                rules.Add("F_DigitLen", new string[] { "小数位数", string.Empty });
                rules.Add("F_ColWidth", new string[] { "显示列宽", string.Empty });
                var data = fieldapp.GetList(string.Empty, list[i].F_Id);
                System.Data.DataTable dt = ListToDataTable(data, rules);
                CreateSheet(workBook, list[i].F_TableTitle + "(" + list[i].F_TableName + ")", dt);
            }
            //写入数据流
            workBook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            string filename = "表定义" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        private void CreateSheet(IWorkbook workBook, string sheetName, System.Data.DataTable table)
        {
            //FileStream fs = new FileStream(this._filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            MemoryStream ms = new MemoryStream();
            ISheet sheet = workBook.CreateSheet(sheetName);
            //处理表格标题
            IRow row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(string.Empty);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            row.Height = 500;

            ICellStyle cellStyle = workBook.CreateCellStyle();
            IFont font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 14;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            //row.Cells[0].CellStyle = cellStyle;

            //处理表格列头
            row = sheet.CreateRow(0);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                row.Height = 350;
                row.Cells[i].CellStyle = cellStyle;
                sheet.AutoSizeColumn(i);
            }

            //处理数据内容
            for (int i = 0; i < table.Rows.Count; i++)
            {
                row = sheet.CreateRow(1 + i);
                row.Height = 250;
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                    sheet.SetColumnWidth(j, 256 * 15);
                }
            }

            //写入数据流
            workBook.Write(ms);
            //ms.Flush();
            //ms.Close();
        }

        #region 字段操作

        public ActionResult FieldForm()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFieldGridJson(string keyword, string tableId)
        {
            var data = fieldapp.GetList(keyword, tableId);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFieldGridJson4ListShow(string keyword, string tableId)
        {
            var data = fieldapp.GetList4ListShow(keyword, tableId);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFieldFormJson(string keyValue)
        {
            var data = fieldapp.GetForm(keyValue);

            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFieldForm(SysFieldDef entity, string keyValue, string tableId)
        {
            entity.F_TableDef_ID = tableId;
            entity.F_DepartmentId = OperatorProvider.Provider.GetCurrent().DepartmentId;
            fieldapp.SubmitForm(entity, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFieldForm(string keyValue)
        {
            fieldapp.DeleteForm(keyValue);
            return Success("删除成功。");
        }

        #endregion 字段操作
    }
}