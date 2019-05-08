using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace ZHXY.Common
{
    public class NPOIExcel
    {
        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <param name="table">  </param>
        /// <returns>  </returns>
        public bool ToExcel(DataTable table)
        {
            var fs = new FileStream(_filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            IWorkbook workBook = new HSSFWorkbook();
            _sheetName = _sheetName.IsEmpty() ? "sheet1" : _sheetName;
            var sheet = workBook.CreateSheet(_sheetName);

            //处理表格标题
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(_title);
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            row.Height = 500;

            var cellStyle = workBook.CreateCellStyle();
            var font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 17;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            row.Cells[0].CellStyle = cellStyle;

            //处理表格列头
            row = sheet.CreateRow(1);
            for (var i = 0; i < table.Columns.Count; i++)
            {
                row.CreateCell(i).SetCellValue(table.Columns[i].ColumnName);
                row.Height = 350;
                sheet.AutoSizeColumn(i);
            }

            //处理数据内容
            for (var i = 0; i < table.Rows.Count; i++)
            {
                row = sheet.CreateRow(2 + i);
                row.Height = 250;
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                    sheet.SetColumnWidth(j, 256 * 15);
                }
            }

            //写入数据流
            workBook.Write(fs);
            fs.Flush();
            fs.Close();

            return true;
        }

        public MemoryStream ToExcelStream(DataTable table, string sheetName)
        {
            if (table == null)
                throw new Exception("无数据");
            _sheetName = sheetName;
            return ToExcelStream(table);
        }

        /// <summary>
        ///     导出到Excel流
        /// </summary>
        /// <param name="users"></param>
        /// <param name="table">  </param>
        /// <returns>  </returns>
        public MemoryStream ToExcelStream(object users, DataTable table)
        {
            //FileStream fs = new FileStream(this._filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var ms = new MemoryStream();
            IWorkbook workBook = new HSSFWorkbook();
            _sheetName = _sheetName.IsEmpty() ? "sheet1" : _sheetName;
            var sheet = workBook.CreateSheet(_sheetName);

            //处理表格标题
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(_title);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            row.Height = 500;

            var cellStyle = workBook.CreateCellStyle();
            var font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 14;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            //row.Cells[0].CellStyle = cellStyle;

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
            //ms.Flush();
            //ms.Close();

            return ms;
        }

        /// <summary>
        ///     将excel导入到datatable
        /// </summary>
        /// <param name="filePath">     excel路径 </param>
        /// <param name="isColumnName"> 第一行是否是列名 </param>
        /// <returns> 返回datatable </returns>
        public static DataTable ExcelToDataTable(string filePath, bool isColumnName)
        {
            DataTable dataTable = null;
            FileStream fs = null;
            DataColumn column = null;
            DataRow dataRow = null;
            IWorkbook workbook = null;
            ISheet sheet = null;
            IRow row = null;
            ICell cell = null;
            var startRow = 0;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        sheet = workbook.GetSheetAt(0); //读取第一个sheet，当然也可以循环读取每个sheet
                        dataTable = new DataTable();
                        if (sheet != null)
                        {
                            var rowCount = sheet.LastRowNum; //总行数
                            if (rowCount > 0)
                            {
                                var firstRow = sheet.GetRow(0); //第一行
                                int cellCount = firstRow.LastCellNum; //列数

                                //构建datatable的列
                                if (isColumnName)
                                {
                                    startRow = 1; //如果第一行是列名，则从第二行开始读取
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        cell = firstRow.GetCell(i);
                                        if (cell?.StringCellValue != null)
                                        {
                                            column = new DataColumn(cell.StringCellValue);
                                            dataTable.Columns.Add(column);
                                        }
                                    }
                                }
                                else
                                {
                                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                                    {
                                        column = new DataColumn("column" + (i + 1));
                                        dataTable.Columns.Add(column);
                                    }
                                }

                                //填充行
                                for (var i = startRow; i <= rowCount; ++i)
                                {
                                    row = sheet.GetRow(i);
                                    if (row == null) continue;

                                    dataRow = dataTable.NewRow();
                                    for (int j = row.FirstCellNum; j < cellCount; ++j)
                                    {
                                        cell = row.GetCell(j);
                                        if (cell == null)
                                            dataRow[j] = "";
                                        else
                                            switch (cell.CellType)
                                            {
                                                case CellType.Blank:
                                                    dataRow[j] = "";
                                                    break;

                                                case CellType.Numeric:
                                                    var format = cell.CellStyle.DataFormat;
                                                    //对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                                    if (format == 14 || format == 31 || format == 57 || format == 58)
                                                        dataRow[j] = cell.DateCellValue;
                                                    else
                                                        dataRow[j] = cell.NumericCellValue;
                                                    break;

                                                case CellType.String:
                                                    dataRow[j] = cell.StringCellValue;
                                                    break;
                                            }
                                    }

                                    dataTable.Rows.Add(dataRow);
                                }
                            }
                        }
                    }
                }

                return dataTable;
            }
            catch
            {
                fs?.Close();
                return null;
            }
        }

        /// <summary>
        ///     导出到Excel
        /// </summary>
        /// <param name="table">      </param>
        /// <param name="title">      </param>
        /// <param name="sheetName">  </param>
        /// <param name="filePath"></param>
        /// <returns>  </returns>
        public bool ToExcel(DataTable table, string title, string sheetName, string filePath)
        {
            _title = title;
            _sheetName = sheetName;
            _filePath = filePath;
            return ToExcel(table);
        }

        public MemoryStream ToExcelStream(DataTable table, string title, string sheetName)
        {
            _title = title;
            _sheetName = sheetName;
            return ToExcelStream(table);
        }

        private void CreateSheet(IWorkbook workBook, string sheetName, string title, DataTable table)
        {
            sheetName = sheetName.IsEmpty() ? "sheet1" : sheetName;
            var sheet = workBook.CreateSheet(sheetName);

            //处理表格标题
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(title);
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            row.Height = 500;

            var cellStyle = workBook.CreateCellStyle();
            var font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 14;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            row.Cells[0].CellStyle = cellStyle;

            //处理表格列头
            row = sheet.CreateRow(1);
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
                row = sheet.CreateRow(2 + i);
                row.Height = 250;
                for (var j = 0; j < table.Columns.Count; j++)
                {
                    row.CreateCell(j).SetCellValue(table.Rows[i][j].ToString());
                    sheet.SetColumnWidth(j, 256 * 15);
                }
            }
        }

        /// <summary>
        ///     导出到Excel流
        /// </summary>
        /// <param name="sheets">  </param>
        /// <returns>  </returns>
        public MemoryStream ToExcelMuchSheet(List<SheetItem> sheets)
        {
            //FileStream fs = new FileStream(this._filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var ms = new MemoryStream();
            var workBook = new HSSFWorkbook();
            foreach (var sheet in sheets)
                CreateSheet(workBook, sheet.SheetName, sheet.Title, sheet.dt);

            //写入数据流
            workBook.Write(ms);
            //ms.Flush();
            //ms.Close();

            return ms;
        }

        /// <summary>
        ///     导出到Excel流
        /// </summary>
        /// <param name="table">  </param>
        /// <returns>  </returns>
        public MemoryStream ToExcelStream(DataTable table)
        {
            //FileStream fs = new FileStream(this._filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            var ms = new MemoryStream();
            var workBook = new HSSFWorkbook();
            _sheetName = _sheetName.IsEmpty() ? "sheet1" : _sheetName;
            var sheet = workBook.CreateSheet(_sheetName);

            //处理表格标题
            var row = sheet.CreateRow(0);
            row.CreateCell(0).SetCellValue(_title);
            //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, table.Columns.Count - 1));
            row.Height = 500;

            var cellStyle = workBook.CreateCellStyle();
            var font = workBook.CreateFont();
            font.FontName = "微软雅黑";
            font.FontHeightInPoints = 14;
            cellStyle.SetFont(font);
            cellStyle.VerticalAlignment = VerticalAlignment.Center;
            cellStyle.Alignment = HorizontalAlignment.Center;
            //row.Cells[0].CellStyle = cellStyle;

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
            //ms.Flush();
            //ms.Close();

            return ms;
        }

        private string _filePath;
        private string _sheetName;

        private string _title;

        public class SheetItem
        {
            public DataTable dt { get; set; }
            public string Title { get; set; }
            public string SheetName { get; set; }
        }
    }
}