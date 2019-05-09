using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ZHXY.Common;

namespace ZHXY.Application
{
    [LoginAuthentication]
    [ValidataParam]
    public abstract class ZhxyWebControllerBase : Controller
    {
        #region property

        protected ILog FileLog => Logger.GetLogger(GetType().ToString());

        #endregion property

        #region View

        [HttpGet]
        [HandlerAuthorize]
        public virtual async Task<ViewResult> Index() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Form() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Details() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Approve() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Return() => await Task.Run(() => View());

        [HttpGet]
        public virtual async Task<ViewResult> Import() => await Task.Run(() => View());

        #endregion View

        #region JsonResult

        protected virtual ActionResult Error(string message) => Content(new { state = ResultState.Error, message = message }.ToJson());

        protected virtual ActionResult Message(string message) => Content(new { state = ResultState.Success, message = message }.ToJson());

        public ActionResult AjaxResult(object data = null) => Content(new { data, state = ResultState.Success }.ToJson());

        public ActionResult Result(object data = null) => Content(data.ToJson());

        /// <summary>
        /// 分页结果
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="records">总记录数</param>
        /// <param name="total">总页数</param>
        /// <returns></returns>
        public ActionResult PagingResult(object data, int records, int total) => Content(new { rows = data, records = records, total = total, state = ResultState.Success }.ToJson());
        public ActionResult PagingResult(object data, Pagination pagination) => Content(new { rows = data, total = pagination.Total, page = pagination.Page, records = pagination.Records }.ToJson());
        
        #endregion JsonResult

        #region others

        protected string CreateExportSql(string tableId, IDictionary<string, string> parms)
        {
            var tableApp = new SysTableDefAppService();
            var fieldApp = new SysFieldDefAppService();
            var stList = tableApp.GetByTName(tableId);
            if (stList.Count != 1)
                throw new Exception("表名重复");
            var st = stList.First();
            var fields = fieldApp.GetList4Export(string.Empty, st.F_Id);
            var i = 0;
            var selectSql = new StringBuilder();
            selectSql.Append("select ");
            var whereSql = new StringBuilder();
            whereSql.Append(" where 1=1 ");
            var joinSql = new StringBuilder();
            foreach (var field in fields)
            {
                if (!field.F_IsExcelDispaly)
                {
                    i++;
                    continue;
                }
                var columnId = field.F_FieldName;
                var columnText = field.F_FieldTitle;
                var dic = field.F_Dic;
                if (!dic.IsEmpty())
                {
                    var tableName = "t" + i;

                    switch (dic)
                    {
                        case "F_OrganizeId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_Organize " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_DepartmentId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_Organize " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_RoleId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_Role " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_DutyId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_Role " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_AreaId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_Area " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_SchoolAreaId":
                            selectSql.Append(tableName + ".F_FullName as '" + columnText + "'");
                            joinSql.Append(" left join School_Area " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_UserId":
                            selectSql.Append(tableName + ".F_RealName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_User " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_ZK_Id":
                            selectSql.Append(tableName + ".F_Title as '" + columnText + "'");
                            joinSql.Append(" left join School_Exam " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        case "F_Student_Name":
                            selectSql.Append(tableName + ".F_Name as '" + columnText + "'");
                            joinSql.Append(" left join School_Students " + tableName + " on t." + columnId + " = " + tableName + ".F_Id");
                            break;

                        default:
                            selectSql.Append(tableName + ".F_ItemName as '" + columnText + "'");
                            joinSql.Append(" left join Sys_ItemsDetail " + tableName + " on t." + columnId + " = " + tableName + ".F_ItemCode and " + tableName + ".F_ItemId = '" + dic + "'");
                            break;
                    }
                }
                else
                {
                    if (columnId == "School_ExamTitle_Entity.F_Title")
                    {
                        columnId = "F_Title";
                    }
                    selectSql.Append(" t." + columnId + " as '" + columnText + "'");
                }
                if (i < (fields.Count - 1))
                    selectSql.Append(" ,");
                i++;
            }

            foreach (var kv in parms)
            {
                whereSql.Append(" and t." + kv.Key + " = @" + kv.Key);
            }

            selectSql.Append(" from " + st.F_TableName + " t");
            var exportSql = selectSql.Append(joinSql).Append(whereSql).ToString();
            return exportSql;
        }

        protected DbParameter[] CreateParms(IDictionary<string, string> parms) => parms.Select(kp => new SqlParameter("@" + kp.Key, kp.Value)).Cast<DbParameter>().ToArray();

        protected List<T> ExcelToList<T>(string filePath, IDictionary<string, string[]> rules)
            where T : new()
        {
            FileStream fs = null;
            IWorkbook workbook = null;
            var list = new List<T>();

            if (!System.IO.File.Exists(filePath))
                return null;
            try
            {
                using (fs = System.IO.File.OpenRead(filePath))
                {
                    //upd by ben
                    // 2007版本
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);
                    if (workbook == null) return null;
                    var sheet = workbook.GetSheetAt(0);
                    if (sheet == null) return list;
                    var rowCount = sheet.LastRowNum;//总行数
                    if (rowCount < 1) return list;
                    var firstRow = sheet.GetRow(0);//第一行
                    var cellCount = firstRow.LastCellNum;//列数

                    //循环创建规则

                    ////建表头
                    //foreach (KeyValuePair<string, string[]> kv in rules) {
                    //}
                    var keys = rules.Keys.ToArray();

                    //填充行
                    for (var i = 1; i <= rowCount; ++i)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null || row.Cells.Count <= 0) continue;
                        var tmp = new T();
                        for (int j = row.FirstCellNum; j < cellCount; ++j)
                        {
                            if (j < 0)
                                break;
                            var cell = row.GetCell(j);
                            var pi = tmp.GetType().GetProperty(keys[j]);
                            if (cell == null)
                            {
                                if (pi != null) pi.SetValue(tmp, null);
                            }
                            else
                            {
                                string[] rule = null;
                                switch (cell.CellType)
                                {
                                    case CellType.Blank:
                                        pi.SetValue(tmp, null);
                                        break;

                                    case CellType.Numeric:

                                        if (pi.PropertyType.FullName.ToLower().Contains("string"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, GetValue(rule, cell.NumericCellValue.ToString(CultureInfo.CurrentCulture)));
                                            else
                                                pi.SetValue(tmp, cell.NumericCellValue.ToString(CultureInfo.CurrentCulture));
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                        {
                                            if (rules.TryGetValue(keys[j], out rule))
                                                pi.SetValue(tmp, Str2Bool(GetValue(rule, cell.NumericCellValue.ToString(CultureInfo.CurrentCulture))));
                                            else
                                                pi.SetValue(tmp, Str2Bool(cell.NumericCellValue.ToString(CultureInfo.CurrentCulture)));
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
                                            pi.SetValue(tmp,
                                                rules.TryGetValue(keys[j], out rule)
                                                    ? GetValue(rule, cell.StringCellValue)
                                                    : cell.StringCellValue);
                                        }
                                        else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                        {
                                            pi.SetValue(tmp,
                                                rules.TryGetValue(keys[j], out rule)
                                                    ? Str2Bool(GetValue(rule, cell.StringCellValue))
                                                    : Str2Bool(cell.StringCellValue));
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

                                    case CellType.Unknown:
                                        break;

                                    case CellType.Formula:
                                        break;

                                    case CellType.Boolean:
                                        break;

                                    case CellType.Error:
                                        break;

                                    default:
                                        throw new ArgumentOutOfRangeException();
                                }
                            }
                        }
                        list.Add(tmp);
                    }
                    return list;
                }
            }
            catch (Exception)
            {
                fs?.Close();
                return null;
            }
        }

        protected List<T> ExcelToList<T>(string filePath, string tableId)
            where T : new()
        {
            var tableApp = new SysTableDefAppService();
            var stList = tableApp.GetByTName(tableId);
            if (stList.Count != 1) throw new Exception("表名重复");
            var st = stList.First();
            FileStream fs = null;
            IWorkbook workbook = null;
            var list = new List<T>();
            if (!System.IO.File.Exists(filePath)) return null;
            try
            {
                using (fs = System.IO.File.OpenRead(filePath))
                {
                    // 2007版本
                    if (filePath.IndexOf(".xlsx", StringComparison.Ordinal) > 0)
                        workbook = new XSSFWorkbook(fs);
                    // 2003版本
                    else if (filePath.IndexOf(".xls", StringComparison.Ordinal) > 0)
                        workbook = new HSSFWorkbook(fs);

                    if (workbook != null)
                    {
                        var sheet = workbook.GetSheetAt(0);
                        if (sheet != null)
                        {
                            var rowCount = sheet.LastRowNum;//总行数
                                                            //if (rowCount > 1)
                                                            //{
                            var firstRow = sheet.GetRow(0);//第一行

                            //页头
                            var cells = firstRow.Cells;
                            //校验页头
                            var fields = new SysFieldDefAppService().GetList(string.Empty, st.F_Id);
                            var importFields = new Dictionary<int, FieldDef>();
                            for (var i = 0; i < cells.Count; i++)
                            {
                                if (string.IsNullOrEmpty(cells[i].StringCellValue))
                                    continue;
                                var f = fields.Where(t => t.F_FieldTitle == cells[i].StringCellValue.Trim()).ToList();
                                if (f.Count != 1)
                                {
                                    throw new Exception("导入模版的字段:" + cells[i].StringCellValue + "没有定义！请在表定义中增加字段！");
                                }

                                importFields.Add(i, f.First());
                            }

                            //填充行
                            for (var i = 1; i <= rowCount; ++i)
                            {
                                var row = sheet.GetRow(i);
                                if (row == null || row.Cells.Count <= 0) continue;
                                var tmp = new T();

                                foreach (var importField in importFields)
                                {
                                    var cell = row.GetCell(importField.Key);
                                    if (cell == null)
                                        continue;
                                    var pi = tmp.GetType().GetProperty(importField.Value.F_FieldName == "School_ExamTitle_Entity.F_Title" ? "F_Title" : importField.Value.F_FieldName);

                                    switch (cell.CellType)
                                    {
                                        case CellType.Blank:
                                            pi.SetValue(tmp, null);
                                            break;

                                        case CellType.Numeric:

                                            if (pi.PropertyType.FullName.ToLower().Contains("string"))
                                            {
                                                if (!importField.Value.F_Dic.IsEmpty())
                                                    pi.SetValue(tmp, GetValue(importField.Value, cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)));
                                                else
                                                    pi.SetValue(tmp, cell.NumericCellValue.ToString(CultureInfo.InvariantCulture));
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                            {
                                                if (!importField.Value.F_Dic.IsEmpty())
                                                    pi.SetValue(tmp, Str2Bool(GetValue(importField.Value, cell.NumericCellValue.ToString())));
                                                else
                                                    pi.SetValue(tmp, Str2Bool(cell.NumericCellValue.ToString(CultureInfo.InvariantCulture)));
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("date"))
                                            {
                                                if (!cell.NumericCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToDateTime(cell.NumericCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("numeric"))
                                            {
                                                if (!cell.NumericCellValue.IsEmpty())
                                                    pi.SetValue(tmp, cell.NumericCellValue);
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("decimal"))
                                            {
                                                if (!cell.NumericCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToDecimal(cell.NumericCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("int"))
                                            {
                                                if (!cell.NumericCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToInt32(cell.NumericCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("double"))
                                            {
                                                if (!cell.NumericCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToDouble(cell.NumericCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else
                                            {
                                                pi.SetValue(tmp, null);
                                            }
                                            break;

                                        case CellType.String:
                                            if (pi.PropertyType.FullName.ToLower().Contains("string"))
                                            {
                                                if (!importField.Value.F_Dic.IsEmpty())
                                                    pi.SetValue(tmp, GetValue(importField.Value, cell.StringCellValue));
                                                else
                                                    pi.SetValue(tmp, cell.StringCellValue);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("boolean"))
                                            {
                                                if (!importField.Value.F_Dic.IsEmpty())
                                                    pi.SetValue(tmp, Str2Bool(GetValue(importField.Value, cell.StringCellValue)));
                                                else
                                                    pi.SetValue(tmp, Str2Bool(cell.StringCellValue));
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("date"))
                                            {
                                                if (!cell.StringCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToDateTime(cell.StringCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("numeric"))
                                            {
                                                pi.SetValue(tmp, cell.NumericCellValue);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("decimal"))
                                            {
                                                if (!cell.StringCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToDecimal(cell.StringCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else if (pi.PropertyType.FullName.ToLower().Contains("int"))
                                            {
                                                if (!cell.StringCellValue.IsEmpty())
                                                    pi.SetValue(tmp, Convert.ToInt32(cell.StringCellValue));
                                                else
                                                    pi.SetValue(tmp, null);
                                            }
                                            else
                                            {
                                                pi.SetValue(tmp, null);
                                            }
                                            break;

                                        default:

                                            break;
                                    }
                                }
                                list.Add(tmp);
                            }
                            //}
                        }
                        if (list.Count <= 0)
                            throw new Exception("未找到数据，或模板错误！");
                        return list;
                    }
                }
                return null;
            }
            catch (Exception e)
            {
                fs?.Close();
                throw e;
            }
        }

        protected object GetPropertyValue(object info, string field)
        {
            if (info == null) return null;
            var t = info.GetType();
            var property = from pi in t.GetProperties() where string.Equals(pi.Name, field, StringComparison.CurrentCultureIgnoreCase) select pi;
            return property.First().GetValue(info, null);
        }

        protected DataTable ListToDataTable<T>(IList<T> list, IDictionary<string, string[]> rules)
            where T : class
        {
            if (list == null || list.Count <= 0)
            {
                return new DataTable();
            }

            var dt = new DataTable(typeof(T).Name);
            var createColumn = true;

            foreach (var t in list)
            {
                if (t == null)
                {
                    continue;
                }

                var row = dt.NewRow();

                //循环创建规则
                foreach (var kv in rules)
                {
                    var pi = typeof(T).GetProperty(kv.Key);
                    var colType = pi.PropertyType;
                    if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    {
                        colType = colType.GetGenericArguments()[0];
                    }
                    if (createColumn)
                    {
                        var column = colType.Name.Equals("Boolean") ? new DataColumn(kv.Value[0], string.Empty.GetType()) : new DataColumn(kv.Value[0], colType);
                        column.AllowDBNull = true;
                        dt.Columns.Add(column);
                    }

                    if (pi.GetValue(t).IsEmpty())
                    {
                        row[kv.Value[0]] = System.DBNull.Value;
                    }
                    else
                    {
                        if (kv.Value[1].IsEmpty())
                        {
                            row[kv.Value[0]] = pi.GetValue(t);
                        }
                        else
                        {
                            object value2;
                            switch (kv.Value[1])
                            {
                                case "F_OrganizeId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.ORGANIZE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_DepartmentId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.ORGANIZE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_RoleId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.ROLE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_DutyId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.DUTY).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_AreaId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.AREA).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SmartCampusConsts.DATAITEMS).TryGetValue(kv.Value[1], out value2))
                                        {
                                            string key;
                                            if (pi.GetValue(t).GetType().Name.Equals("Boolean"))
                                            {
                                                key = pi.GetValue(t).ToBool() ? "1" : "0";
                                            }
                                            else
                                            {
                                                key = pi.GetValue(t).ToString();
                                            }

                                            ((Dictionary<string, string>)value2).TryGetValue(key, out var value3);
                                            row[kv.Value[0]] = value3;
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }

                if (createColumn)
                {
                    createColumn = false;
                }

                dt.Rows.Add(row);
            }

            return dt;
        }

        protected string GetPdf(List<Dictionary<string, string>> para, string sourcePdf, string descPdfFileName)
        {
            if (descPdfFileName.IsEmpty())
                descPdfFileName = NumberBuilder.Build_18bit() + ".pdf";
            ITextSharpHelper.GetTemplateToPDFByList(sourcePdf, Server.MapPath(Configs.GetValue("modelPath2")), descPdfFileName, para);
            return Configs.GetValue("modelPath2") + descPdfFileName;
        }


        private object GetValue(IReadOnlyList<string> rules, object value)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            if (rules[1].IsEmpty())
            {
                return value;
            }

            var value2 = string.Empty;
            SysCacheAppService.GetOrganizeListByCache();
            switch (rules[1])
            {
                case "F_RoleId":
                    value2 = SysCacheAppService.GetRoleListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    break;

                case "F_DutyId":
                    value2 = SysCacheAppService.GetDutyListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    break;

                case "F_AreaId":
                    value2 = SysCacheAppService.GetAreaListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    break;

                default:
                    if (SysCacheAppService.GetDataItemListByCache().TryGetValue(rules[1], out var dictionaryItem))
                    {
                        value2 = ((Dictionary<string, string>)dictionaryItem)
                            .FirstOrDefault(q => q.Value.Equals(value)).Key;
                    }
                    break;
            }
            return value2;
        }

        private object GetValue(FieldDef importFields, object value)
        {
            CacheFactory.Cache();

            if (importFields.F_Dic.IsEmpty())
            {
                return value;
            }

            var orgs = SysCacheAppService.GetOrganizeListByCache();

            var value2 = (string)value;

            if ("F_OrganizeId".Equals(importFields.F_Dic))
            {
                value2 = orgs.FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
            }

            switch (importFields.F_Dic)
            {
                case "F_RoleId":
                    {
                        if (importFields.F_TableDef_ID == "Sys_User" || importFields.F_TableDef_ID == "d34d98a9-1174-4333-b99b-5c59c82733a0")
                        {
                            return value2;
                        }

                        value2 = SysCacheAppService.GetRoleListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    };
                    break;

                case "F_DutyId":
                    value2 = SysCacheAppService.GetDutyListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    break;

                case "F_AreaId":
                    value2 = SysCacheAppService.GetAreaListByCache().FirstOrDefault(q => GetPropertyValue(q.Value, "fullname").Equals(value)).Key;
                    break;

                default:
                    {
                        if (!importFields.F_Dic_Code.IsEmpty())

                        {
                            if (SysCacheAppService.GetDataItemListByCache().TryGetValue(importFields.F_Dic_Code, out var dictionaryItem))
                            {
                                value2 = ((Dictionary<string, string>)dictionaryItem)
                                    .FirstOrDefault(q => q.Value.Equals(value)).Key;
                            }
                        }

                        break;
                    }
            }
            return value2;
        }

        private bool Str2Bool(object value)
        {
            switch (value)
            {
                case "true":
                    return true;

                case "false":
                case "1":
                case "0":
                    return false;

                default:
                    return false;
            }
        }

        #endregion others
    }
}