using System.Collections.Generic;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemManage.Controllers
{
    /// <summary>
    /// 数据字典管理
    /// [OK]
    /// </summary>
    public class ItemsDataController : ZhxyWebControllerBase
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
        public ActionResult SubmitForm(SysDicItem itemsDetailEntity, string keyValue)
        {
            var str = App.SubmitForm(itemsDetailEntity, keyValue);
            if (str.IsEmpty())
            {
                return Message("操作成功。");
            }
            else
            {
                return Error(str);
            }
        }

        [HttpPost]
        
        public ActionResult DeleteForm(string keyValue)
        {
            var F_Id = keyValue.Split('|');
            for (var i = 0; i < F_Id.Length - 1; i++)
            {
                App.DeleteForm(F_Id[i]);
            }
            return Message("删除成功。");
        }



       
    }
}