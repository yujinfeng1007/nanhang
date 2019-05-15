using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleController : ZhxyWebControllerBase
    {
        private RoleService App { get; }
        public RoleController(RoleService app) => App = app;


        [HttpGet]

        public ActionResult Load(string keyword)
        {
            var data = App.GetList(keyword);
            return Resultaat.Success(data);
        }


        [HttpGet]

        public ActionResult Get(string id)
        {
            var data = App.GetById(id);
            return Resultaat.Success(data);
        }

        [HttpPost]

        public ActionResult Update(UpdateRoleDto dto)
        {
            App.Update(dto);
            return Resultaat.Success();
        }

        [HttpPost]

        public ActionResult Add(AddRoleDto dto)
        {
            App.Add(dto);
            return Resultaat.Success();
        }

        [HttpPost]

        public ActionResult Delete(string[] id)
        {
            App.Delete(id);
            return Resultaat.Success();
        }
    }
}