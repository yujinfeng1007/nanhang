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
        public List<Dic> GetAll() => Read<Dic>().ToList();

        public Dic GetById(string id) => Get<Dic>(id);


        public void Add(DicDto dto)
        {
            var dic = dto.MapTo<Dic>();
            AddAndSave(dic);
        }
        public void Delete(string id)
        {
            if (Read<DicItem>(p => p.Code.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            DelAndSave<Dic>(id);
        }

        public dynamic GetList()
        {
            return Read<Dic>().OrderBy(p => p.SortCode).OrderBy(p => p.Type).Select(p =>
                      new
                      {
                          p.Code,
                          p.Type,
                          p.Name,
                          p.SortCode,
                      }).ToListAsync().Result;
        }

        public void Update(DicDto dto)
        {
            var dic = Get<Dic>(dto.Code);
            dto.MapTo(dic);
            SaveChanges();
        }


        public object GetData()
        {
            var data = new Dictionary<string, Dictionary<string, string>>();
            var dics = Read<Dic>().ToList();
            dics.ForEach(item =>
            {
                var items = Read<DicItem>(p => p.Code.Equals(item.Code)).ToDictionary(p => p.Key, e => e.Value);
                data.Add(item.Code, items);
            });

            data.Add("orgList", Read<Organ>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("dutyList", Read<Duty>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("roleList", Read<Role>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("resourceList", Read<Resource>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            return data;
        }


        public void AddItem(DicItemDto dto)
        {
            var item = dto.MapTo<DicItem>();
            AddAndSave(item);
        }

        public void UpdateItem(DicItemDto dto)
        {
            var item = Get<DicItem>(dto.Code);
            dto.MapTo(item);
            SaveChanges();
        }

        public void DeleteItem(string[] id)
        {
            var removeList = Query<DicItem>(p => id.Contains(p.Id)).ToList();
            DelAndSave<DicItem>(removeList);
        }

        public dynamic GetItems(string code)
        {
            return Read<DicItem>(p => p.Code.Equals(code)).ToListAsync().Result;
        }

        public List<DicItem> GetAllItems()
        {
            return Read<DicItem>().ToListAsync().Result;
        }
    }
}