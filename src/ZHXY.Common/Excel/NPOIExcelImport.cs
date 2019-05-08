using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace ZHXY.Common
{
    public static class NPOIExcelImport<T>
        where T : new()
    {
        public static List<T> GetDatas(ISheet sheet, IDictionary<string, Func<string, object>> rules)
        {
            var list = new List<T>();
            IRow row = null;
            ICell cell = null;
            if (sheet != null)
            {
                var rowCount = sheet.LastRowNum; //总行数
                if (rowCount > 1)
                {
                    var firstRow = sheet.GetRow(0); //第一行
                    int cellCount = firstRow.LastCellNum; //列数

                    //循环创建规则

                    ////建表头
                    //foreach (KeyValuePair<string, string[]> kv in rules) {
                    //}
                    var keys = rules.Keys.ToArray();

                    //填充行
                    for (var i = 1; i <= rowCount; ++i)
                    {
                        row = sheet.GetRow(i);
                        if (row == null || row.Cells.Count <= 0) continue;
                        var tmp = new T();
                        Func<string, object> rule = null;
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j < 0)
                                break;
                            cell = row.GetCell(j);
                            var pi = tmp.GetType().GetProperty(keys[j]);
                            if (cell == null)
                                pi.SetValue(tmp, null);
                            else
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        pi.SetValue(tmp, null);
                                        break;

                                    case CellType.Numeric:
                                        // short format = cell.CellStyle.DataFormat;
                                        ////对时间格式（2015.12.5、2015/12/5、2015-12-5等）的处理
                                        //if (format == 14 || format == 31 || format == 57 || format == 58)
                                        //    pi.SetValue(tmp, cell.DateCellValue);
                                        //else
                                        //    pi.SetValue(tmp, cell.NumericCellValue);
                                        if (pi.PropertyType.FullName.ToLower().Contains("string"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, rule(cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)));
                                            else
                                                pi.SetValue(tmp, cell.NumericCellValue.ToString(CultureInfo.InvariantCulture));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, rule(cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)));
                                            else
                                                pi.SetValue(tmp, rule(cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("date"))
                                        {
                                            pi.SetValue(tmp, Convert.ToDateTime(cell.NumericCellValue));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("numeric"))
                                        {
                                            pi.SetValue(tmp, cell.NumericCellValue);
                                        }
                                        else
                                        {
                                            pi.SetValue(tmp, null);
                                        }

                                        break;

                                    case CellType.String:
                                        if (pi.PropertyType.FullName.ToLower().Contains("string"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, rule(cell.StringCellValue));
                                            else
                                                pi.SetValue(tmp, cell.StringCellValue);
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, rule(cell.StringCellValue));
                                            else
                                                pi.SetValue(tmp, rule(cell.StringCellValue));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("date"))
                                        {
                                            pi.SetValue(tmp, Convert.ToDateTime(cell.StringCellValue));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("numeric"))
                                        {
                                            pi.SetValue(tmp, cell.NumericCellValue);
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("int"))
                                        {
                                            pi.SetValue(tmp, Convert.ToInt32(cell.StringCellValue));
                                        }
                                        else
                                        {
                                            pi.SetValue(tmp, null);
                                        }

                                        break;
                                }
                        }

                        list.Add(tmp);
                    }
                }
            }

            return list;
        }

        public static List<SheetModel> ImportToList(string filePath, IDictionary<string, Func<string, object>> rules)
        {
            FileStream fs = null;
            var datas = new List<SheetModel>();
            if (!File.Exists(filePath))
                return null;
            try
            {
                using (fs = File.OpenRead(filePath))
                {
                    IWorkbook workbook = null;
                    // 2007版本
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        var sheetCount = workbook.NumberOfSheets; //读取第一个sheet，当然也可以循环读取每个sheet
                        for (var i = 0; i < sheetCount; i++)
                        {
                            var sheet = workbook.GetSheetAt(i);
                            var list = GetDatas(sheet, rules);
                            datas.Add(new SheetModel
                            {
                                Datas = list,
                                SheetName = sheet.SheetName
                            });
                        }
                    }
                }

                return datas;
            }
            catch
            {
                fs?.Close();
                return null;
            }
        }

        public class SheetModel
        {
            public List<T> Datas { get; set; }
            public string SheetName { get; set; }
        }

        public static bool WriteExcel(string MoudleFilePath, string DataFilePath, List<DHStudentMoudle> StudentInfosList)
        {
            FileStream fs = null;
            if (!File.Exists(MoudleFilePath)) { File.Create(MoudleFilePath); }
            if (File.Exists(DataFilePath)) { File.Delete(DataFilePath); }
            try
            {
                using (fs = File.OpenRead(MoudleFilePath))
                {
                    IWorkbook workbook = null;
                    // 2007版本
                    if (MoudleFilePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                    {
                        workbook = new XSSFWorkbook(fs);
                    }
                    // 2003版本
                    else if (MoudleFilePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                    {
                        workbook = new HSSFWorkbook(fs);
                    }
                    if (workbook != null)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        //开始写入数据
                        for(int i=0; i<StudentInfosList.Count; i++)
                        {
                            var rows = sheet.CreateRow(i + 1);
                            var student = StudentInfosList[i];
                            rows.CreateCell(0).SetCellValue(student.name);
                            rows.CreateCell(1).SetCellValue(student.sex);
                            rows.CreateCell(2).SetCellValue(student.StudentNum);
                            rows.CreateCell(3).SetCellValue(student.OrgName);
                            rows.CreateCell(4).SetCellValue(student.CredNum);
                            rows.CreateCell(5).SetCellValue(student.type);
                            rows.CreateCell(10).SetCellValue(student.ColleageCode);
                            rows.CreateCell(11).SetCellValue(student.BuildName);
                            rows.CreateCell(12).SetCellValue(student.FloorName);
                            rows.CreateCell(13).SetCellValue(student.DormName);
                        }
                        using (fs = File.OpenWrite(DataFilePath))
                        {
                            workbook.Write(fs);
                        }
                        fs.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                string ErrorMess = ex.Message;
                Console.WriteLine(ErrorMess);
                fs?.Close();
                return false;
            }
            return true;
        }
    }
}