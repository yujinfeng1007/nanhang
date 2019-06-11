using System;
using System.Linq;
using System.Web.Http;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Api.Controllers
{
    /// <summary>
    /// 南航大屏数据接口
    /// </summary>
    public class DataViewController : BaseApiController
    {
        private IDeviceService App { get; }

        public DataViewController(IDeviceService app) => App = app;

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
                var relevance = App.Read<Relevance>(t => t.Name == "Device_Building" && t.FirstKey == input.F_Sn).FirstOrDefault();

                var buildings = App.GetBindGate(relevance);

                var json = new
                {
                    F_Is_Binded = relevance != null ? true : false,
                    F_Buildings = buildings.Select(t => new { F_BuildingId = t.Id, F_BuildingName = t.BuildingNo }),
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
                App.BindBuildings(input.F_Sn, input.F_BuildingIds);

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
                App.UnBindBuildings(input.F_Sn);

                return Success("Ok");

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.4.	检测是否有新的版本信息
        /// </summary>
        /// <returns></returns>
        public IHttpActionResult CheckAppVersion(CheckVersionInput input)
        {
            try
            {
                return Success(App.GetLatestAppVersion(input.F_currentVersion));
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
                return Success(App.GetDormOutInInfo(input.F_BuildingIds));

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
                return Success(App.GetVisitorList(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.7.	根据楼栋获取进出最近记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetLatestInOutRecordByBuilding(GateDto input)
        {
            try
            {
                return Success(App.GetLatestInOutRecord(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }

        /// <summary>
        /// 1.8.	根据最近24小时进出最近记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetInOutNumInLatestHours(GateDto input)
        {
            try
            {
                return Success(App.GetInOutNumInLatestHours(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }


        /// <summary>
        /// 1.9.	根据楼栋获取考勤统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult GetSignInfoByBuildings(GateDto input)
        {
            try
            {
                return Success(App.GetSignInfo(input.F_BuildingIds));

            }
            catch (Exception e)
            {
                return Error("0001", e.Message);
            }
        }
    }
}
