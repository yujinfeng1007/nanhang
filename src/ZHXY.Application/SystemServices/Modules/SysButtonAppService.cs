using System;
using System.Collections.Generic;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 模块按钮管理
    /// </summary>
    public class SysButtonAppService : AppService
    {
        public SysButtonAppService(IZhxyRepository r) : base(r)
        {
        }
        public List<SysButton> GetList(string moduleId = "")
        {
            var expression = ExtLinq.True<SysButton>();
            if (!string.IsNullOrEmpty(moduleId))
            {
                expression = expression.And(t => t.F_ModuleId == moduleId);
            }
            return Read<SysButton>(expression).OrderBy(t => t.F_SortCode).ToList();
        }

        public SysButton GetForm(string keyValue) => Get<SysButton>(keyValue);

        public void DeleteForm(string keyValue)
        {
            if (Read<SysButton>().Count(t => t.F_ParentId.Equals(keyValue)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                DelAndSave<SysButton>(t => t.F_Id == keyValue);
            }
        }

        public void SubmitForm(SysButton moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
               var data= Get<SysButton>(keyValue);
                moduleButtonEntity.MapTo(data);
                data.F_Id = keyValue;
                SaveChanges();
            }
            else
            {
                moduleButtonEntity.F_Id= Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(moduleButtonEntity);
            }
        }

        public void SubmitCloneButton(string moduleId, string Ids)
        {
            var ArrayId = Ids.Split(',');
            var data = GetList();
            //var entitys = new List<SysButton>();
            foreach (var item in ArrayId)
            {
                var moduleButtonEntity = data.Find(t => t.F_Id == item);
                moduleButtonEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                moduleButtonEntity.F_ModuleId = moduleId;
                //entitys.Add(moduleButtonEntity);
                AddAndSave<SysButton>(moduleButtonEntity);
            }
            SaveChanges();
            //Repository.SubmitCloneButton(entitys);
        }
    }
}