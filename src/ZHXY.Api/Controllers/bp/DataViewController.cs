using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ZHXY.Application;
using ZHXY.Application.DormServices.Gates.Dto;
using ZHXY.Domain;

namespace ZHXY.Api.Controllers.bp
{
    /// <summary>
    /// 大屏数据统计
    /// </summary>
    public class DataViewController : BaseApiController
    {
        private GateAppService app = new GateAppService(new ZhxyRepository());

        private DormBuildingService dormBuildingApp = new DormBuildingService(new ZhxyRepository());

        /// <summary>
        /// 1.1.	查询可绑定的闸机信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IHttpActionResult GetUnBindGate(GateDto input)
        {
            try
            {
                var gates = app.GetList();

                var buildings = app.GetBuildings();

                var json = new
                {
                    F_Is_Binded = false,
                    F_Gates = gates,
                    F_Buildings = buildings
                };
                return Success(json);

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
           
        }

        /// <summary>
        /// 1.2.	大屏绑定闸机接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IHttpActionResult BindGate(GateDto input)
        {
            try
            {

                // dormBuildingApp.BindBuilding(input);

                // var building = dormBuildingApp.GetBuildingByNo(input.F_GateId);

                //var json = new
                //{
                //    F_BuildingId = building.Id,
                //    F_BuildingName = building.Title
                //};
                //return Success(json);
                return Success("");

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.3.	大屏解绑闸机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IHttpActionResult UnbindGate(GateDto input)
        {
            try
            {

               // dormBuildingApp.UnbindBuilding(input);

                return Success();

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }
    }
}
