using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Dorm.Controllers
{
    public class FaceController : ZhxyController
    {
       
        public FaceService App { get; }
        public FaceController(FaceService app) => App = app;

        [HttpPost]
        // <summary>
        // 头像审批申请 
        // </summary>     
        public dynamic Apply(FaceRequestDto input)
        {
            var approveFilepath = string.Empty;//审批后的头像
            var existen = string.Empty;
            var mapPath = ConfigurationManager.AppSettings["MapPath"] + DateTime.Now.ToString("yyyyMMdd") + "/";
            var basePath = Server.MapPath(mapPath);
            var files = System.Web.HttpContext.Current.Request.Files;
            if (files.Count > 0)
            {
                if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                var random = RandomHelper.GetRandom();
                var todayStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                for (var i = 0; i < files.Count; i++)
                {
                    var strRandom = random.Next(1000, 10000).ToString(); //生成编号
                    var uploadName = $"{todayStr}{strRandom}";
                    existen = files[i].FileName.Substring(files[i].FileName.LastIndexOf('.') + 1);

                    var fullPath = $"{basePath}{uploadName}.{existen}";
                    files[i].SaveAs(fullPath);
                    approveFilepath = $"http://{Request.Url.Host}:{Request.Url.Port}{mapPath}{uploadName}.{existen}";
                   // approveFilepath = $"{mapPath}{uploadName}.{existen}";
                }
            }

            App.Request(input, approveFilepath);



            return Result.Success();
        }

        /// <summary>
        /// 获取头像审批列表
        /// </summary>
        [HttpGet]
        public ActionResult GetList(GetFaceApprovalListDto input) {
          // input.CurrentUserId =  Operator.Current.Id;
            var data = App.GetFaceApprovalList(input);          
            return Result.PagingRst(data, input.Records, input.Total);


        }
        /// <summary>
        /// 获取头像审批详情
        /// </summary>
        [HttpGet]       
        public ActionResult Get(string appId, string currentUserId)
        {
            var data = App.GetFaceApprovalDetail(appId, currentUserId);
            return Result.Success(data);
        }


        /// <summary>
        /// 头像审批
        /// </summary>
        [HttpPost]
        public ActionResult Approval(FaceApprovalDto input)
        {
            //input.CurrentUserId = Operator.Current.Id;
            App.Approval(input);
            return Result.Success();
        }

       


    }
}