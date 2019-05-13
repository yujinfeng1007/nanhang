using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.Dorm.Controllers
{
    public class StudentController : ZhxyWebControllerBase
    {

        private StudentAppService App { get; }

        public StudentController(StudentAppService app) => App = app;

        public ActionResult UpdSchool_Index() => View();

        public ActionResult Index_gb() => View();

        public ActionResult Form_gb() => View();

        public ActionResult Details_gb() => View();

        public ActionResult CollegeIndex() => View();
        public ActionResult CollegeDetails() => View();


        [HttpGet]

        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_Grade, string F_Class, string F_Year)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword, F_DepartmentId, F_Grade, F_Class, F_Year),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetGridSelect()
        {
            var list = new List<Student>();
            foreach (var item in OperatorProvider.Current.Classes)
            {
                foreach (var item1 in App.GetListByClassId(item.Key))
                {
                    list.Add(item1);
                }
            }

            return Content(list.ToJson());
        }

        /// <summary>
        /// 根据班级ID查学生
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]

        public ActionResult GetGridSelectByClassId(string F_ClassID)
        {
            var list = App.GetListByClassId(F_ClassID);
            return Content(list.ToJson());
        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        /// <summary>
        /// </summary>
        /// <param name="keyValue"> 学号、id </param>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult GetFormJsonByStuNo(string keyValue)
        {
            var data = App.GetBykeyValue(keyValue);
            return Content(data.ToJson());
        }

        public ActionResult GetDetailsJson(string keyValue)
        {
            var data = App.Get(keyValue);

            var obj = new object();
            var dic = CacheFactory.Cache().GetCache<Dictionary<string, object>>(SYS_CONSTS.ORGANIZE);
            if (data.F_Class_ID != null && dic.TryGetValue(data.F_Class_ID, out obj))
            {
                data.F_Class_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            if (data.F_Divis_ID != null && dic.TryGetValue(data.F_Divis_ID, out obj))
            {
                data.F_Divis_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            if (data.F_Grade_ID != null && dic.TryGetValue(data.F_Grade_ID, out obj))
            {
                data.F_Grade_ID = GetPropertyValue(obj, "fullname").ToString();
            }
            var t = data.ToJson();
            return Content(data.ToJson());
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Student entity, string keyValue)
        {
            //entity.F_DepartmentId = OperatorProvider.Current.DepartmentId;
            App.SubmitForm(entity, keyValue);
            return Message("操作成功。");
        }

        [HttpPost]



        [HandlerAuthorize]


        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            return Message("删除成功。");
        }

        public ActionResult Details(string keyValue) => View();

        /// <summary>
        /// 调整在线状态
        /// </summary>
        /// <returns>  </returns>
        [HttpGet]
        public ActionResult UpdCurStatu() => View();

        [HttpPost]

        [HandlerAuthorize]

        public ActionResult SubmitCurStatu(string keyValue, string F_CurStatu)
        {
            return Message("调整在线状态成功。");
        }
        #region Model

        public class Honor_Info
        {
            public string F_Id { get; set; }
            public string F_Title { get; set; }
            public string F_Covers { get; set; }
            public string F_Content { get; set; }
            public DateTime? F_Date { get; set; }
        }

        public class StudentModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_StudentNum { get; set; }
            public string F_CardNo { get; set; }
            public string F_HeadPic { get; set; }
            public string F_Sex { get; set; }
            public DateTime? F_Birthday { get; set; }
            public string F_Introduction { get; set; }
            public List<Honor_Info> F_honorInfo { get; set; }
        }

        #endregion Model
    }
}