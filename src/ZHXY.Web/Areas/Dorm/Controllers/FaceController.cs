using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Application.DormServices.Face.Dto;

namespace ZHXY.Web.Dorm.Controllers
{
    public class FaceController : ZhxyWebControllerBase
    {
       
        public FaceService App { get; }
        public FaceController(FaceService app) => App = app;

        [HttpPost]
        // <summary>
        // 头像审批申请
        // </summary>     
        public dynamic Apply(FaceRequestDto input)
        {
            App.Request(input);



            return Resultaat.Success();
        }

        //[HttpPost]
        //public dynamic Apply(string userId)
        //{
        //    // App.Request(input);
        //    string data = userId;
        //    return Resultaat.Success();
        //}

    }
}