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
    public class SysDicAppService : AppService
    {
        public SysDicAppService(IZhxyRepository r) : base(r)
        {
            
        }

        public SysDicAppService()
        {
            R = new ZhxyRepository();
        }

        public List<SysDic> GetAll() =>Read<SysDic>().ToList();

        public SysDic GetById(string id) => Get<SysDic>(id);

        public void Delete(string id)
        {
            if(Read<SysDic>(p=>p.F_ParentId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
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
    }
}