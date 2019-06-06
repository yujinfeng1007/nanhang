using System.Linq;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 节假日设置
    /// </summary>
    public class HolidayController : ZhxyController
    {
        private HolidayAppService App { get; }
        public HolidayController(HolidayAppService app) => App = app;

        [HttpGet]
        public ActionResult Get(string id)
        {
           return Result.Success(App.GetById(id));
        }
        public ActionResult Update(UpdateHolidayDto input)
        {
            App.Update(input);
            return Result.Success();
        }

        [HttpGet]
        public ActionResult Load(Pagination pag,string keyword)
        {
            var rows = App.Load(pag,  keyword);
            return Result.PagingRst(rows, pag.Records, pag.Total);
        }

        public ActionResult GetList()
        {
            return Result.Success(App.Read<Holiday>().ToList().Serialize());
        }

        [HttpPost]
        public ActionResult Add(AddHolidayDto dto)
        {
            App.Add(dto);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                App.Delete(id.Split(','));
            }
            return Result.Success();
        }

    }
}