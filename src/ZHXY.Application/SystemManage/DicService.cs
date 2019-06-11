using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class DicService : AppService
    {
        public DicService(DbContext r) : base(r)  {  }

        public List<Dic> GetAll() => Read<Dic>().ToList();

        public Dic GetById(string keyValue) => Get<Dic>(keyValue);

        public void Delete(string id)
        {
            if (Read<Dic>().Count(t => t.ParentId.Equals(id)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                DelAndSave<Dic>(t => t.Id == id);
            }
        }

        public void Submit(Dic itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = Get<Dic>(keyValue);
                itemsEntity.MapTo(data);
                data.Id=(keyValue);
                SaveChanges();
            }
            else
            {
                itemsEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(itemsEntity);
            }
        }

        public List<DicItem> GetItemList(string itemId = "", string keyword = "")
        {
            var query = Read<DicItem>();
            query = string.IsNullOrEmpty(itemId) ? query : query.Where(t => t.ItemId == itemId);
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.Name.Contains(keyword) || t.Code.Contains(keyword));
            return query.OrderBy(t => t.Sort).ToListAsync().Result;
        }

        public List<DicItem> GetItemListByCode(string enCode)
        {
            var data = Read<Dic>(t => t.Code == enCode).FirstOrDefault();
            if (data == null)
                return new List<DicItem>();
            return Read<DicItem>(t => t.ItemId == data.Id).ToList();
        }

        public DicItem GetItemById(string id) => Get<DicItem>(id);

        public void DeleteItem(string keyValue) => DelAndSave<DicItem>(t => t.Id == keyValue);

        public string SubmitItem(DicItem itemsDetailEntity, string keyValue)
        {
            var list = new List<DicItem>();
            var expression = ExtLinq.True<DicItem>();
            if (!string.IsNullOrEmpty(keyValue))
            {
                expression = expression.And(t => t.ItemId == itemsDetailEntity.ItemId);
                expression = expression.And(t => t.Code == itemsDetailEntity.Code);
                expression = expression.And(t => t.Id != keyValue);
                list = Read(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                var data = Get<Dic>(keyValue);
                itemsDetailEntity.MapTo(data);
                data.Id = (keyValue);
                SaveChanges();
            }
            else
            {
                expression = expression.And(t => t.ItemId == itemsDetailEntity.ItemId);
                expression = expression.And(t => t.Code == itemsDetailEntity.Code);
                list = Read(expression).ToList();
                if (list.Count > 0)
                    return "重复的编码";
                itemsDetailEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(itemsDetailEntity);
            }

            return "";
        }
        public object GetDataItemList()
        {
            var itemDetails = GetItemList();
            var dic = new Dictionary<string, object>();
            foreach (var item in GetAll())
            {
                var tempDictionary = new Dictionary<string, string>();
                var details = itemDetails.FindAll(t => t.ItemId.Equals(item.Id));
                foreach (var i in details)
                {
                    try
                    {
                        tempDictionary.Add(i.Code, i.Name);
                    }
                    catch
                    {
                        // ignored
                    }
                }
                dic.Add(item.Code, tempDictionary);
            }
            return dic;
        }

        public Dictionary<string, object> GetDataItemListByCache()
        {
            if (!RedisCache.KeyExists(SysConsts.DATAITEMS))
            {
                var itemDetails = GetItemList();
                var dic = new Dictionary<string, object>();
                foreach (var item in GetAll())
                {
                    var tempDictionary = new Dictionary<string, string>();
                    var details = itemDetails.FindAll(t => t.ItemId.Equals(item.Id));
                    foreach (var i in details)
                    {
                        try
                        {
                            tempDictionary.Add(i.Code, i.Name);
                        }
                        catch
                        {
                        }
                    }
                    dic.Add(item.Code, tempDictionary);
                }
                RedisCache.Set(SysConsts.DATAITEMS, dic);
            }
            return RedisCache.Get<Dictionary<string, object>>(SysConsts.DATAITEMS);
        }

        public Dictionary<string, object> GetDutyListByCache()
        {
            if (!RedisCache.KeyExists(SysConsts.DUTY))
            {
                var data = GetItemListByCode("Duty");
                var dictionary = new Dictionary<string, object>();
                foreach (var item in data)
                {
                    var fieldItem = new
                    {
                        encode = item.Code,
                        fullname = item.Name
                    };
                    dictionary.Add(item.Code, fieldItem);
                }
                RedisCache.Set(SysConsts.DUTY, dictionary);
            }

            return RedisCache.Get<Dictionary<string, object>>(SysConsts.DUTY);
        }



    }
}