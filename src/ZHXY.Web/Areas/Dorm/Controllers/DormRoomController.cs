using ZHXY.Application;
using System.Web.Mvc;

namespace ZHXY.Web.Dorm.Controllers
{

    public class DormRoomController : BaseController
    {
        private DormRoomAppService App { get; }
        public DormRoomController(DormRoomAppService app) => App = app;

        [HttpGet]
        public ActionResult Load(Pagination p)
        {
            var rows = App.Load(p);
            return Result.PagingRst(rows, p.Records, p.Total);
        }

        [HttpGet]
        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Result.Success(data);
        }

    }
}