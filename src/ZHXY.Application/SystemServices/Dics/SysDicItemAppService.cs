using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典项管理
    /// </summary>
    public class SysDicItemAppService : AppService
    {
        public SysDicItemAppService(DbContext r) : base(r)
        {
        }

        public List<SysDicItem> GetList(string itemId = "", string keyword = "")
        {
            var query = Read<SysDicItem>();
            query = string.IsNullOrEmpty(itemId) ? query : query.Where(t => t.F_ItemId == itemId );
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.F_ItemName.Contains(keyword) || t.F_ItemCode.Contains(keyword));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }

        public List<SysDicItem> GetItemList(string enCode)
        {
            var data = Read<SysDic>(t=>t.F_EnCode==enCode).FirstOrDefault();
            if (data == null)
                return new List<SysDicItem>();
           return Read<SysDicItem>(t => t.F_ItemId == data.F_Id).ToList();
        }

        //public List<SysDicItem> GetItemByEnCode(string enCode) => GetItemList(enCode);

        public SysDicItem Get(string id) => Get<SysDicItem>(id);

        public void DeleteForm(string keyValue) =>DelAndSave<SysDicItem>(t => t.F_Id == keyValue);

        public string SubmitForm(SysDicItem itemsDetailEntity, string keyValue)
        {
            var list = new List<SysDicItem>();
            var expression = ExtLinq.True<SysDicItem>();
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.F_ItemId == itemsDetailEntity.F_ItemId);
                expression = expression.And(t => t.F_ItemCode == itemsDetailEntity.F_ItemCode);
                expression = expression.And(t => t.F_Id != keyValue);
                list =Read< SysDicItem >(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                var data = Get<SysDic>(keyValue);
                itemsDetailEntity.MapTo(data);
                data.F_Id=(keyValue);
                SaveChanges();
            }
            else
            {
                expression = expression.And(t => t.F_ItemId == itemsDetailEntity.F_ItemId);
                expression = expression.And(t => t.F_ItemCode == itemsDetailEntity.F_ItemCode);
                list = Read<SysDicItem>(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                itemsDetailEntity.F_Id= Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave<SysDicItem>(itemsDetailEntity);
            }

            return "";
        }
    }
}