using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Web.Dorm.Controllers
{
    public class StudentController : BaseController
    {

        private StudentService App { get; }

        public StudentController(StudentService app) => App = app;

        public ActionResult Load(Pagination pag, string keyword)
        {
            var data = App.GetList(pag, keyword);
            return Result.PagingRst(data, pag.Records, pag.Total);
        }

        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

        [HttpGet]

        public ActionResult GetByStudentNumber(string number)
        {
            var data = App.GetByStudentNumber(number);
            return Result.Success(data);
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




    }
}