using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class TableDefController : ZhxyWebControllerBase
    {
        private SysTableDefAppService App { get; }
        private SysFieldDefAppService FieldApp { get; }

        public TableDefController(SysTableDefAppService app, SysFieldDefAppService fieldApp)
        {
            App = app;
            FieldApp = fieldApp;
        }

        [HttpGet]
        
        public ActionResult GetGridJson(string keyword)
        {
            var data = App.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(TableDef entity, string keyValue)
        {
            entity.F_DepartmentId = OperatorProvider.Current.DepartmentId;
            App.SubmitForm(entity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            FieldApp.DeleteByTableId(keyValue);
            return Message("删除成功。");
        }

        //导出excel
        [HttpGet]
        [HandlerAuthorize]
        public FileResult export(string keyValue)
        {
            /////////////////获得数据集合
            var list = App.GetList();
            return ToMuchSheetExcel(list);
        }

        public FileResult ToMuchSheetExcel(List<TableDef> list)
        {
            var ms = new MemoryStream();
            IWorkbook workBook = new HSSFWorkbook();
            for (var i = 0; i < list.Count; i++)
            {
                IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
                rules.Add("F_FieldName", new string[] { "字段名称", string.Empty });
                rules.Add("F_FieldTitle", new string[] { "字段标题", string.Empty });
                rules.Add("F_DataType", new string[] { "数据类型", string.Empty });
                rules.Add("F_Length", new string[] { "数据长度", string.Empty });
                rules.Add("F_DigitLen", new string[] { "小数位数", string.Empty });
                rules.Add("F_ColWidth", new string[] { "显示列宽", string.Empty });
                var data = FieldApp.GetList(string.Empty, list[i].F_Id);
                var dt = ListToDataTable(data, rules);
                CreateSheet(workBook, list[i].F_TableTitle + "(" + list[i].F_TableName + ")", dt);
            }
            //写入数据流
            workBook.Write(ms);
            ms.Seek(0, SeekOrigin.Begin);
            var filename = "表定义" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            return File(ms, "application/ms-excel", filename);
        }

        private void CreateSheet(IWorkbook workBook, string sheetName, System.Data.DataTable table)
        {
            var ms = new MemoryStream();
            var sheet = workBook.CreateSheet(sheetName);
            //处理表格标题
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(string.Empty);
            row.Height = 500;

            var cellStyle = workBook.CreateCellStyle();
            var font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 14;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;

            //处理表格列头
            row = sheet.CreateRow(0);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                row.Height = 350;
                row.Cells[i].CellStyle = cellStyle;
                sheet.AutoSizeColumn(i);
            }

            //处理数据内容
            for (var i = 0; i < table.Rows.Count; i++)
            {
                row = sheet.CreateRow(1 + i);
                row.Height = 250;
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                    sheet.SetColumnWidth(j, 256 * 15);
                }
            }

            //写入数据流
            workBook.Write(ms);
        }

        #region 字段操作

        public ActionResult FieldForm() => View();

        [HttpGet]
        
        public ActionResult GetFieldGridJson(string keyword, string tableId)
        {
            var data = FieldApp.GetList(keyword, tableId);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFieldGridJson4ListShow(string keyword, string tableId)
        {
            var data = FieldApp.GetList4ListShow(keyword, tableId);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFieldFormJson(string keyValue)
        {
            var data = FieldApp.Get(keyValue);

            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFieldForm(FieldDef entity, string keyValue, string tableId)
        {
            entity.F_TableDef_ID = tableId;
            entity.F_DepartmentId = OperatorProvider.Current.DepartmentId;
            FieldApp.SubmitForm(entity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFieldForm(string keyValue)
        {
            FieldApp.DeleteForm(keyValue);
            return Message("删除成功。");
        }

        #endregion 字段操作
    }
}