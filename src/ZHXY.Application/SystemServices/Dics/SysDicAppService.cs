using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Domain.Entity;

namespace ZHXY.Application
{
    /// <summary>
    /// 字典管理
    /// </summary>
    public class SysDicAppService : AppService
    {
        public SysDicAppService(DbContext r) : base(r)
        {
        }

        public List<SysDic> GetList() => Read<SysDic>().ToList();

        public SysDic GetForm(string keyValue) => Get<SysDic>(keyValue);

        public void DeleteForm(string keyValue)
        {
            if (Read<SysDic>().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                DelAndSave<SysDic>(t => t.F_Id == keyValue);
            }
        }

        public void SubmitForm(SysDic itemsEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = Get<SysDic>(keyValue);
                itemsEntity.MapTo(data);
                data.F_Id=(keyValue);
                SaveChanges();
            }
            else
            {
                itemsEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper(); 
                AddAndSave<SysDic>(itemsEntity);
            }
        }
    }
}