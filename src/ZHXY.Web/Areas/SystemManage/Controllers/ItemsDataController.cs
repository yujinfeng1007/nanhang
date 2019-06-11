using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;
using ZHXY.Domain.Entity;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典管理
    /// [OK]
    /// </summary>
    public class ItemsDataController : BaseController
    {
        private DicService App { get;}
        public ItemsDataController(DicService app) => App = app;

        [HttpGet]
        
        public ActionResult List(string itemId, string keyword)
        {
            var data = App.GetItemList(itemId, keyword);
            return Result.PagingRst(data);
        }

        [HttpGet]
        
        public ActionResult GetSelectJson(string enCode)
        {
            var data = App.GetItemListByCode(enCode);
            var list = new List<object>();
            foreach (var item in data)
            {
                list.Add(new { id = item.Code, text = item.Name });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        
        public ActionResult GetFormJson(string keyValue)
        {
            var data = App.GetItemById(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DicItem itemsDetailEntity, string keyValue)
        {
            var str = App.SubmitItem(itemsDetailEntity, keyValue);
            if (str.IsEmpty())
            {
                RedisCache.Remove(SysConsts.DATAITEMS);
                RedisCache.Set( SysConsts.DATAITEMS, App.GetDataItemList());
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
                App.DeleteItem(F_Id[i]);
            }
            RedisCache.Remove(SysConsts.DATAITEMS);
            RedisCache.Set( SysConsts.DATAITEMS, App.GetDataItemList());
           return Result.Success();
        }
    }
}