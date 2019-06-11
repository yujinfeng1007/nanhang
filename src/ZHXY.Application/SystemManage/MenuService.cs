using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{

    /// <summary>
    /// 模块管理
    /// </summary>
    public class MenuService : AppService
    {
        private RoleAuthorizeService RoleAuthorizeApp { get; }

        public MenuService(DbContext r, RoleAuthorizeService roleAuthorizeApp) : base(r)
        {
            RoleAuthorizeApp = roleAuthorizeApp;
        }
        public List<Module> GetList() =>Read< Module >().OrderBy(t => t.Sort).ToList();

        public List<Module> GetEnableList() =>Read< Module >(t => t.Enabled == true).OrderBy(t => t.Sort).ToList();

        public Module GetById(string id) =>  Get<Module>(id) ;

        public void Delete(string id)
        {
            if (Read<Module>().Count(t => t.ParentId == id) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            DelAndSave<Module>(t => t.Id == id);
        }

        public void Submit(Module moduleEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = Get<Module>(keyValue);
                moduleEntity.MapTo(data);
                data.Id = keyValue;
                SaveChanges();
            }
            else
            {
                moduleEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(moduleEntity);
            }
        }


        public object GetMenuListByType(string clientType)
        {

            if (Operator.GetCurrent().IsSystem)
            {
                return ToMenuJson(RoleAuthorizeApp.GetMenuList("0", clientType), "0");
            }
            var roles = Operator.GetCurrent().Roles;
            var data = new List<Module>();
            foreach (var e in roles)
            {
                data = data.Union(RoleAuthorizeApp.GetMenuList(e, clientType)).ToList();
            }
            return ToMenuJson(data, "0");
        }

        public object GetMenuList(string clientType)
        {
            clientType = string.IsNullOrEmpty(clientType) ? "2" : clientType;
            return GetMenuListByType(clientType);
        }

        public static string ToMenuJson(List<Module> data, string parentId)
        {
            var sbJson = new StringBuilder();
            sbJson.Append("[");
            var entitys = data.FindAll(t => t.ParentId == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    var strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id) + string.Empty);
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        // 菜单按钮
        public object GetMenuButtonList()
        {
            var roles = Operator.GetCurrent().Roles;
            var data = new List<Button>();
            foreach (var e in roles)
            {
                data = data.Union(RoleAuthorizeApp.GetButtonList(e)).ToList();
            }

            var dataModuleId = data.Distinct(new ExtList<Button>("F_ModuleId"));
            var dictionary = new Dictionary<string, object>();
            foreach (var item in dataModuleId)
            {
                var buttonList = data.Where(t => t.ModuleId.Equals(item.ModuleId)).ToList();
                dictionary.Add(item.ModuleId, buttonList);
            }
            return dictionary;
        }


        public List<Button> GetButtonList(string moduleId = "")
        {
            var expression = ExtLinq.True<Button>();
            if (!string.IsNullOrEmpty(moduleId))
            {
                expression = expression.And(t => t.ModuleId == moduleId);
            }
            return Read(expression).OrderBy(t => t.Sort).ToList();
        }

        public Button GetButtonById(string id) => Get<Button>(id);

        public void DeleteButton(string id)
        {
            if (Read<Button>().Count(t => t.ParentId.Equals(id)) > 0)
            {
                throw new Exception("删除失败！操作的对象包含了下级数据。");
            }
            else
            {
                DelAndSave<Button>(t => t.Id == id);
            }
        }

        public void SubmitButton(Button moduleButtonEntity, string keyValue)
        {
            if (!string.IsNullOrEmpty(keyValue))
            {
                var data = Get<Button>(keyValue);
                moduleButtonEntity.MapTo(data);
                data.Id = keyValue;
                SaveChanges();
            }
            else
            {
                moduleButtonEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                AddAndSave(moduleButtonEntity);
            }
        }

        public void SubmitCloneButton(string moduleId, string Ids)
        {
            var ArrayId = Ids.Split(',');
            var data = GetButtonList();
            foreach (var item in ArrayId)
            {
                var moduleButtonEntity = data.Find(t => t.Id == item);
                moduleButtonEntity.Id = Guid.NewGuid().ToString("N").ToUpper();
                moduleButtonEntity.ModuleId = moduleId;
                AddAndSave(moduleButtonEntity);
            }
            SaveChanges();
        }
    }
}