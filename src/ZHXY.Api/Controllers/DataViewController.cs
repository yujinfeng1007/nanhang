using System;
using System.Linq;
using System.Web.Http;
using ZHXY.Application;
using ZHXY.Application.DormServices.Gates.Dto;
using ZHXY.Domain;

namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 南航大屏数据接口
    /// </summary>
    public class DataViewController : BaseApiController
    {
        private RelevanceService rApp = new RelevanceService(new ZhxyRepository());
        private DeviceService app = new DeviceService(new ZhxyRepository());
        //private DormBuildingService app = new DormBuildingService(new ZhxyRepository());

        /// <summary>
        /// 1.1.	查询可绑定的楼栋信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetUnBindGate(GateDto input)
        {
            try
            {
                var relevance = rApp.Read<Relevance>(t=>t.Name== "Device_Building" && t.FirstKey==input.F_Sn).FirstOrDefault();

                var buildings = app.GetBindGate(relevance);

                var json = new
                {
                    F_Is_Binded = relevance == null?false:true,
                    F_Buildings = buildings.Select(t=>new { F_BuildingId= t.Id,F_BuildingName=t.BuildingNo}),
                };
                return Success(json);

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }

        }

        /// <summary>
        /// 1.2.	大屏绑定楼栋信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult BindBuildings(GateDto input)
        {
            try
            {
                app.BindBuildings(input.F_Sn, input.F_BuildingIds);

                return Success("Ok");

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.3.	大屏解绑楼栋信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UnBindBuildings(GateDto input)
        {
            try
            {
                app.UnBindBuildings(input.F_Sn);

                return Success("Ok");

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.5.	获取楼栋学生基本外出在寝信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetBasicDormOutInInfoGroupByBuilld(GateDto input)
        {
            try
            {
                return Success(app.GetDormOutInInfo(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.6.	获取访客清单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetVisitorsList(GateDto input)
        {
            try
            {
                return Success(app.GetVisitorList(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }
    }
}
