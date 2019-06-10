using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;

namespace ZHXY.Web.Dorm.Controllers
{
    /// <summary>
    /// 楼栋管理
    /// </summary>
    public class DormBuildingController : ZhxyController
    {
        private DormBuildingService App { get; }

        public DormBuildingController(DormBuildingService app) => App = app;

        public async Task<ViewResult> BindUsers() => await Task.Run(() => View());

        #region HttpPost

        [HttpPost]
        public async Task<ActionResult> Add(CreateDormBuildingDto dto)
        {

            await App.AddAsync(dto);
            return Result.Success();
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            await App.DelAsync(id);
            return Result.Success();
        }

        [HttpPost]
        public async Task<ActionResult> Update(UpdateDormBuildingDto dto)
        {
            await App.UpdAsync(dto);
            return Result.Success();
        }

        #endregion HttpPost

        #region HttpGet

        [HttpGet]
        public async Task<ActionResult> Get(string id) => await Task.Run(() => Result.Success(App.Get(id)));

        [HttpGet]
        public async Task<ActionResult> GetList(Pagination pagination, string keyword)
        {
            return await Task.Run(() => Result.PagingRst(App.GetList(pagination, keyword), pagination.Records, pagination.Total));
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            return await Task.Run(() => Result.Success(App.GetAll()));
        }

        //获取未绑定的宿管
        [HttpGet]
        public ActionResult GetNotBindUsers(string id)
        {
            var data = App.GetNotBindUsers(id);
            return Result.Success(data);
        }

        
        [HttpPost]
        public ActionResult BindUsers(string id, string[] users)
        {
            App.BindUsers(id, users);
            return Result.Success();
        }

        [HttpPost]
        public ActionResult UnBindUser(string id, string userId)
        {
            App.UnBindUser(id, userId);
            return Result.Success();
        }
        

        [HttpGet]
        public ActionResult SubBindUsers(string id)
        {
            var data = App.GetSubBindUsers(id);
            return Result.Success(data);
        }
       


        #endregion HttpGet
    }
}