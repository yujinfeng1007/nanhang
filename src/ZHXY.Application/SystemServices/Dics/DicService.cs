using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class DicService : AppService
    {
        public DicService(IZhxyRepository r) : base(r) { }
        public List<SysDic> GetAll() => Read<SysDic>().ToList();

        public SysDic GetById(string id) => Get<SysDic>(id);
      

        public void Add(DicDto dto)
        {
            var dic = dto.MapTo<SysDic>();
            AddAndSave(dic);
        }
        public void Delete(string id)
        {
            if (Read<SysDicItem>(p => p.DicId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            DelAndSave<SysDic>(id);
        }

        public dynamic GetList()
        {
           
            return Read<SysDic>().OrderBy(p => p.SortCode).Select(p =>
                    new
                    {
                        p.Id,
                        p.Category,
                        p.Name,
                        p.SortCode,
                    }).ToListAsync().Result;
        }

        public void Update(DicDto dto)
        {
            var dic = Get<SysDic>(dto.Id);
            dto.MapTo(dic);
            SaveChanges();
        }


        public object GetData()
        {
            var data = new Dictionary<string, Dictionary<string, string>>();
            var dics = Read<SysDic>().ToList();
            dics.ForEach(item =>
            {
                var items = Read((SysDicItem p) => p.DicId.Equals(item.Id)).ToDictionary(p=> p.Key,e=> e.Value);
                data.Add(item.Id, items);
            });

            data.Add("orgList", Read<Organ>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("dutyList", Read<Duty>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("roleList", Read<Role>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("menuList", Read<Menu>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("funcList", Read<Function>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            return data;
        }


        public void AddItem(DicItemDto dto)
        {
            var item = dto.MapTo<SysDicItem>();
            AddAndSave(item);
        }

        public void UpdateItem(DicItemDto dto)
        {
            var item = Get<SysDicItem>(dto.DicId);
            dto.MapTo(item);
            SaveChanges();
        }

        public void DeleteItem(string id)
        {
            DelAndSave<SysDicItem>(id);
        }

        public List<SysDicItem> GetItems(string dicId)
        {
            return Read<SysDicItem>(p => p.DicId.Equals(dicId)).ToListAsync().Result;
        }

        public List<SysDicItem> GetAllItems()
        {
            return Read<SysDicItem>().ToListAsync().Result;
        }
    }
}