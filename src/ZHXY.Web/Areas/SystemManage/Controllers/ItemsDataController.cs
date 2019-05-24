using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典管理
    /// [OK]
    /// </summary>
    public class ItemsDataController : ZhxyController
    {
        private SysDicItemAppService App { get;}

        public ItemsDataController(SysDicItemAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = App.GetList(itemId, keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetSelectJson(string enCode)
        {
            var data = App.GetItemList(enCode);
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.Get(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(SysDicItem itemsDetailEntity, string keyValue)
        {
            var str = App.SubmitForm(itemsDetailEntity, keyValue);
            if (str.IsEmpty())
            {
                CacheFactory.Cache().RemoveCache(SmartCampusConsts.DATAITEMS);
                CacheFactory.Cache().WriteCache(CacheService.GetDataItemList(), SmartCampusConsts.DATAITEMS);
                return Result.Success();
            }
            else
            {
                throw new Exception(str);
            }
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                //逻辑删除
                //ItemsDetailEntity itemsDetailEntity = itemsDetailApp.GetForm(F_Id[i]);
                //itemsDetailEntity.Remove();
                App.DeleteForm(F_Id[i]);
            }
            CacheFactory.Cache().RemoveCache(SmartCampusConsts.DATAITEMS);
            CacheFactory.Cache().WriteCache(CacheService.GetDataItemList(), SmartCampusConsts.DATAITEMS);
           return Result.Success();
        }
    }
}