using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        private DormBuildingService app = new DormBuildingService(new ZhxyRepository());

        /// <summary>
        /// 1.1.	查询可绑定的楼栋信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public IHttpActionResult GetUnBindGate(GateDto input)
        {
            try
            {

                var buildings = app.GetBindGate(input.F_Sn);

                var json = new
                {
                    F_Is_Binded = true,
                    F_Buildings = buildings,
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
        public IHttpActionResult BindBuildings(GateDto input)
        {
            try
            {
                app.BindBuildings(input.F_Sn, input.F_BuildingId);

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
        public IHttpActionResult GetBasicDormOutInInfoGroupByBuilld(GateDto input)
        {
            try
            {
                return Success(app.GetDormOutInInfo(input.F_Sn));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }
    }
}
