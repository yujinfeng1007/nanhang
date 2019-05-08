using System;
using System.Configuration;
using System.Net.Http;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace ZHXY.Web.Controllers
{
    public class ConfigController : Controller
    {
        [HttpGet]
        public ActionResult GetSchoolList()
        {
            var qdNo = ConfigurationManager.AppSettings["qdNo"];
            var url = ConfigurationManager.AppSettings["getSchoolListUrl"];
            var result = new HttpClient().GetStringAsync($"{url}{qdNo}").Result;
            var j = JObject.Parse(result);
            var isError = (bool)j.GetValue("IsError", StringComparison.InvariantCultureIgnoreCase);
            if (isError) throw new Exception("获取学校信息失败!");
            var data = j.GetValue("Data", StringComparison.InvariantCultureIgnoreCase)?.ToString();
            return Content(data);
        }
    }
}