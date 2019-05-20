using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Web.Mvc;

namespace ZHXY.Web.Dorm.Controllers
{

    public class InOutReceiveController : ZhxyController
	{
		private InOutReceiveAppService App { get; }

        public InOutReceiveController(InOutReceiveAppService inOutApp)
        {
            App = inOutApp;
        }
		
        [HttpGet]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = App.GetList(pagination),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };

            return Content(data.ToJson());
        }
        
        [HttpGet]
        public ActionResult GetFormJson(string id)
        {
            var data = App.GetById(id);
            return Content(data.ToJson());
        }
        
      
        
        [HttpPost]
        public ActionResult DeleteForm(string id)
        {
            App.Delete(id);
            return Result.Success();
        }
        
	
       
	}
}