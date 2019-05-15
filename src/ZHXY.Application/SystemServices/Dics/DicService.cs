using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class DicService : AppService
    {
        public DicService(IZhxyRepository r) : base(r)
        {

        }

        public DicService()
        {
            R = new ZhxyRepository();
        }

        public List<SysDic> GetAll() => Read<SysDic>().ToList();

        public SysDic GetById(string id) => Get<SysDic>(id);

        public void Delete(string id)
        {
            if (Read<SysDic>(p => p.F_ParentId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            DelAndSave<SysDic>(id);
        }

        public void Submit(SysDic input, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var dic = Get<SysDic>(id);
                input.MapTo(dic);
            }
            else
            {
                input.F_Id = Guid.NewGuid().ToString();
                Add(input);
            }
            SaveChanges();
        }

        public object GetData()
        {
            var data = new Dictionary<string, Dictionary<string, string>>();
            var dics = Read<SysDic>().ToList();
            dics.ForEach(item =>
            {
                var items = Read<SysDicItem>(p => p.F_ItemId.Equals(item.F_Id)).Select(p => new { p.F_ItemCode, p.F_ItemName }).ToDictionary(p=>p.F_ItemCode,e=>e.F_ItemName);
                data.Add(item.F_EnCode, items);
            });

            data.Add("orgList", Read<Organ>().Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("dutyList", Read<Role>(p=>p.Category==2).Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));
            data.Add("roleList", Read<Role>(p=>p.Category==1).Select(p => new { p.Id, p.Name }).ToDictionary(p => p.Id, e => e.Name));

            return data;
        }
    }
}