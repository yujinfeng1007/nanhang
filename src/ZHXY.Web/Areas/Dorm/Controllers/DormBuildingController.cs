using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 楼栋管理
    /// </summary>
    public class DormBuildingController : ZhxyWebControllerBase
    {
        private DormBuildingService App { get; }

        public DormBuildingController(DormBuildingService app) => App = app;

        public async Task<ViewResult> BindUsers() => await Task.Run(() => View());

        #region HttpPost

        [HttpPost]
        public async Task<ActionResult> Add(CreateDormBuildingDto dto)
        {

            await App.AddAsync(dto);
            return Resultaat.Success();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await App.DelAsync(id);
            return Resultaat.Success();
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateDormBuildingDto dto)
        {
            await App.UpdAsync(dto);
            return Resultaat.Success();
        }

        #endregion HttpPost

        #region HttpGet

        [HttpGet]
        public async Task<ActionResult> Get(string id) => await Task.Run(() => Resultaat.Success(App.Get(id)));

        [HttpGet]
        public async Task<ActionResult> GetList(Pagination pagination, string keyword)
        {
            return await Task.Run(() => Resultaat.PagingRst(App.GetList(pagination, keyword), pagination.Records, pagination.Total));
        }

        //获取未绑定的宿管
        [HttpGet]
        public ActionResult GetNotBindUsers(string id)
        {
            var data = App.GetNotBindUsers(id);
            return Resultaat.Success(data);
        }

        
        [HttpPost]
        public ActionResult BindUsers(string id, string[] users)
        {
            App.BindUsers(id, users);
            return Resultaat.Success();
        }

        [HttpPost]
        public ActionResult UnBindUser(string id, string userId)
        {
            App.UnBindUser(id, userId);
            return Resultaat.Success();
        }
        

        [HttpGet]
        public ActionResult SubBindUsers(string id)
        {
            var data = App.GetSubBindUsers(id);
            return Resultaat.Success(data);
        }
       


        #endregion HttpGet
    }
}