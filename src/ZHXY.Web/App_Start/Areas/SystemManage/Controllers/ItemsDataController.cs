using NFine.Application.SystemManage;
using NFine.Code;
using NFine.Domain.Entity.SystemManage;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NFine.Web.Areas.SystemManage.Controllers
{
    public class ItemsDataController : ControllerBase
    {
        private ItemsDetailApp itemsDetailApp = new ItemsDetailApp();
        private ICache cache = CacheFactory.Cache();

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetGridJson(string itemId, string keyword)
        {
            var data = itemsDetailApp.GetList(itemId, keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetSelectJson(string enCode)
        {
            var data = itemsDetailApp.GetItemList(enCode);
            List<object> list = new List<object>();
            foreach (ItemsDetail item in data)
            {
                list.Add(new { id = item.F_ItemCode, text = item.F_ItemName });
            }
            return Content(list.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = itemsDetailApp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(ItemsDetail itemsDetailEntity, string keyValue)
        {
            string str = itemsDetailApp.SubmitForm(itemsDetailEntity, keyValue);
            if (Ext.IsEmpty(str))
            {
                cache.RemoveCache(Cons.DATAITEMS);
                cache.WriteCache(CacheConfig.GetDataItemList(), Cons.DATAITEMS);
                return Success("操作成功。");
            }
            else
            {
                return Error(str);
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                //逻辑删除
                //ItemsDetailEntity itemsDetailEntity = itemsDetailApp.GetForm(F_Id[i]);
                //itemsDetailEntity.Remove();
                itemsDetailApp.DeleteForm(F_Id[i]);
            }
            cache.RemoveCache(Cons.DATAITEMS);
            cache.WriteCache(CacheConfig.GetDataItemList(), Cons.DATAITEMS);
            return Success("删除成功。");
        }
    }
}