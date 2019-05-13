using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.Dorm.Controllers
{
    public class TeacherController : ZhxyWebControllerBase
    {

        private TeacherAppService App { get; }
        public TeacherController(TeacherAppService app) => App = app;

        [HttpGet]

        public ActionResult GetGridJson(Pagination pagination, string keyword, string F_DepartmentId, string F_CreatorTime_Start, string F_CreatorTime_Stop)
        {
            var data = new
            {
                rows = App.GetList(pagination, keyword, F_DepartmentId, F_CreatorTime_Start, F_CreatorTime_Stop),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetGridJsonSelect(string F_Teachers_ID)
        {
            var data = App.GetListSelect(F_Teachers_ID);
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetGridSelect(string F_Name, string F_Divis_ID)
        {
            var data = App.GetSelect(F_Name, F_Divis_ID);
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetById(keyValue);
            ////将用户id替换成姓名
            //var creator = new UserEntity();
            //var modifier = new UserEntity();
            //Dictionary<string, UserEntity> dic = CacheFactory.Cache().GetCache<Dictionary<string, UserEntity>>(Cons.USERS);
            //if (data.F_CreatorUserId != null)
            //{
            //    if (dic.TryGetValue(data.F_CreatorUserId, out creator))
            //    {
            //        data.F_CreatorUserId = creator.F_RealName;
            //    }
            //}
            //if (data.F_LastModifyUserId != null)
            //{
            //    if (dic.TryGetValue(data.F_LastModifyUserId, out modifier))
            //    {
            //        data.F_LastModifyUserId = modifier.F_RealName;
            //    }
            //}
            return Content(data.ToJson());
        }

        [HttpGet]

        public ActionResult GetFormJsonByF_Num(string F_Num)
        {
            var data = App.GetByNum(F_Num);
            return Content(data.ToJson());
        }

        [HttpPost]

        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(Teacher entity, string keyValue)
        {
            App.SubmitForm(entity, keyValue);
            CacheFactory.Cache().RemoveCache(SYS_CONSTS.CLASSTEACHERS);
            return Message("操作成功。");
        }

        [HttpPost]
        [HandlerAuthorize]

        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            CacheFactory.Cache().RemoveCache(SYS_CONSTS.CLASSTEACHERS);
            return Message("删除成功。");
        }

        [HttpGet]

        public ActionResult getTeacherDetailInfo(string F_TeacherId)
        {
            var entity = App.GetById(F_TeacherId);
            var tea = new TeacherModel();
            if (entity != null)
            {
                tea.F_Id = entity.Id;
                tea.F_Name = entity.F_Name;
                tea.F_HeadPic = entity.teacherSysUser.F_HeadIcon;
                tea.F_Sex = entity.F_Gender;
                tea.F_Birthday = entity.F_Birthday;
                tea.F_Introduction = entity.F_Introduction;
                tea.F_MobilePhone = entity.F_MobilePhone;

            }
            return Content(tea.ToJson());
        }

        public ActionResult GetDivisTeacher(string F_Divis_ID)
        {
            var datas = App.GetList(t => t.F_Divis_ID == F_Divis_ID);
            return Content(datas.ToJson());
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

        public class TeacherModel
        {
            public string F_Id { get; set; }
            public string F_Name { get; set; }
            public string F_Subject { get; set; }
            public string F_HeadPic { get; set; }
            public string F_Sex { get; set; }
            public DateTime? F_Birthday { get; set; }
            public string F_Introduction { get; set; }
            public List<Honor_Info> F_honorInfo { get; set; }
            public string F_MobilePhone { get; set; }
        }

        #endregion Model
    }
}