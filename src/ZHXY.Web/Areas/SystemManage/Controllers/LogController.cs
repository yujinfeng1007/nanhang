
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Web.Shared;

namespace ZHXY.Web.SystemManage.Controllers
{
    public class LogController : BaseController
    {
        private LogService App { get; }
        public LogController(LogService app) => App = app;
        public async Task<ViewResult> LoginHis() => await Task.Run(() => View());

        [HttpGet]
        public ActionResult Load(Pagination input)
        {
            var list = App.Load(input);
            return Result.PagingRst(list, input.Records, input.Total);
        }
    }
}