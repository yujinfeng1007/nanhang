﻿
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
namespace ZHXY.Web.Controllers
{
    public class LogController : ZhxyWebControllerBase
    {
        private SysLogAppService App { get; }
        public LogController(SysLogAppService app) => App = app;
        public async Task<ViewResult> LoginHis() => await Task.Run(() => View());

        [HttpGet]
        public ActionResult Load(GetLogListDto input)
        {
            var list = App.Load(input);
            return Resultaat.PagingRst(list, input.Records, input.Total);
        }
    }
}