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
    public class TeacherController : ZhxyController
    {

        private TeacherService App { get; }
        public TeacherController(TeacherService app) => App = app;

        [HttpGet]

        public ActionResult Load(Pagination pag, string keyword)
        {
            var data = App.GetList(pag, keyword);
            return Result.PagingRst(data,pag.Records,pag.Total);
        }

        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpGet]

        public ActionResult GetByJobNumber(string F_Num)
        {
            var data = App.GetByJobNumber(F_Num);
            return Result.Success(data);
        }

        [HttpPost]
        //机构绑定负责人
        public ActionResult TeacherBindClass(string classId, string teacherId)
        {
            App.BindTeacherOrg(classId, teacherId);
            return Result.Success();
        }

        [HttpGet]
        //获取负责人所绑定的班级
        public ActionResult GetBindClass(string teacherId)
        {
            var data =  App.GetBindClass(teacherId);
            return Result.Success(data);
        }

    }
}