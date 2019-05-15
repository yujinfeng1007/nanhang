using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典项管理
    /// </summary>
    public class SysDicItemAppService : AppService
    {
        private IItemDetailRepository Repository { get; }

        public SysDicItemAppService()
        {
            Repository = new ItemsDetailRepository();
            R = new ZhxyRepository();
        }

        public SysDicItemAppService(IItemDetailRepository repos, IZhxyRepository r)
        {
            Repository = repos;
            R = r;
        }

        public List<SysDicItem> GetList(string itemId = "", string keyword = "")
        {
            var query = Read<SysDicItem>();
            query = string.IsNullOrEmpty(itemId) ? query : query.Where(t => t.F_ItemId == itemId && t.F_DeleteMark == false);
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.F_ItemName.Contains(keyword) || t.F_ItemCode.Contains(keyword));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<SysDicItem> GetItemList(string enCode) => Repository.GetItemList(enCode);

        public List<SysDicItem> GetItemByEnCode(string enCode) => Repository.GetItemByEnCode(enCode);

        public SysDicItem Get(string id) => Get<SysDicItem>(id);

        public void DeleteForm(string keyValue) => Repository.BatchDelete(t => t.F_Id == keyValue);

        public string SubmitForm(SysDicItem itemsDetailEntity, string keyValue)
        {
            var list = new List<SysDicItem>();
            var expression = ExtLinq.True<SysDicItem>();
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.F_ItemId == itemsDetailEntity.F_ItemId);
                expression = expression.And(t => t.F_ItemCode == itemsDetailEntity.F_ItemCode);
                expression = expression.And(t => t.F_Id != keyValue);
                list = Repository.QueryAsNoTracking(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                itemsDetailEntity.Modify(keyValue);
                Repository.Update(itemsDetailEntity);
            }
            else
            {
                expression = expression.And(t => t.F_ItemId == itemsDetailEntity.F_ItemId);
                expression = expression.And(t => t.F_ItemCode == itemsDetailEntity.F_ItemCode);
                list = Repository.QueryAsNoTracking(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                itemsDetailEntity.Create();
                Repository.Insert(itemsDetailEntity);
            }

            return "";
        }

       
    }
}