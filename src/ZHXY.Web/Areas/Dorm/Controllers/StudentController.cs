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

        private StudentService App { get; }

        public StudentController(StudentService app) => App = app;

        public ActionResult Load(Pagination pag, string keyword)
        {
            var data = App.GetList(pag, keyword);
            return Resultaat.PagingRst(data, pag.Records, pag.Total);
        }

        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Resultaat.Success(data);
        }

        [HttpGet]

        public ActionResult GetByStudentNumber(string number)
        {
            var data = App.GetByStudentNumber(number);
            return Resultaat.Success(data);
        }


    }
}