using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Data.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 地区管理
    /// </summary>
    public class AreaService : AppService
    {

        public AreaService(IZhxyRepository r) : base(r) { }

        public List<Area> GetAll() => Read<Area>().ToListAsync().Result;

        public List<Area> GetListByParentId(string parentId) => Read<Area>(t => t.ParentId == parentId).ToList();

        public dynamic GetById(string id) => Get<Area>(id);

        public void Delete(string id)
        {
            if (Read<Area>(t => t.ParentId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            
                DelAndSave<Area>(id);
        }

    }
}