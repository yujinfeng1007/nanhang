using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using log4net;
using ZHXY.Common;

namespace ZHXY.Application
{
    //[LoginAuthentication]
    //[ValidateParam]
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


        protected DbParameter[] CreateParms(IDictionary<string, string> parms) => parms.Select(kp => new SqlParameter("@" + kp.Key, kp.Value)).Cast<DbParameter>().ToArray();

      
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
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.ORGANIZE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_DepartmentId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.ORGANIZE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_RoleId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.ROLE).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_DutyId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.DUTY).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                case "F_AreaId":
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.AREA).TryGetValue((string)pi.GetValue(t), out value2))
                                        {
                                            row[kv.Value[0]] = GetPropertyValue(value2, "fullname");
                                        }

                                        break;
                                    }
                                default:
                                    {
                                        if (CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.DATAITEMS).TryGetValue(kv.Value[1], out value2))
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