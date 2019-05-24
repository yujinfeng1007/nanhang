using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.WebPages;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{

    /// <summary>
    /// 模块管理
    /// </summary>
    public class SysModuleAppService:AppService
    {
        public SysModuleAppService(IZhxyRepository r) : base(r)
        {
        }
        public List<SysModule> GetList() =>Read< SysModule >().OrderBy(t => t.F_SortCode).ToList();

        public List<SysModule> GetEnableList() =>Read< SysModule >(t => t.F_EnabledMark == true).OrderBy(t => t.F_SortCode).ToList();

        public SysModule GetForm(string keyValue) =>  Get<SysModule>(keyValue) ;

        public void DeleteForm(string keyValue)
        {
            if (Read<SysModule>().Count(t => t.F_ParentId == keyValue) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            DelAndSave<SysModule>(t => t.F_Id == keyValue);
        }

        public void SubmitForm(SysModule moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = Get<SysModule>(keyValue);
                moduleEntity.MapTo(data);
                data.F_Id = keyValue;
                SaveChanges();
            }
            else
            {
                moduleEntity.F_Id = Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(moduleEntity);
            }
        }
    }
}