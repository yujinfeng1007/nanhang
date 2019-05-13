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

        public ActionResult Load(Pagination pag, string keyword)
        {
            var data = App.GetList(pag, keyword);
            return Resultaat.PagingRst(data,pag.Records,pag.Total);
        }

        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Resultaat.Success(data);
        }

        [HttpGet]

        public ActionResult GetByJobNumber(string F_Num)
        {
            var data = App.GetByJobNumber(F_Num);
            return Resultaat.Success(data);
        }

    }
}