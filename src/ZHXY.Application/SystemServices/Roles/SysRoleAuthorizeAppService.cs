using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

using ZHXY.Common;

namespace ZHXY.Application
{
    public class SysRoleAuthorizeAppService : AppService
    {
        private IRepositoryBase<RoleAuthorize> Repository { get; }
        private SysModuleAppService ModuleApp { get; }
        private SysButtonAppService ModuleButtonApp { get; }

        public SysRoleAuthorizeAppService()
        {
            Repository = new Repository<RoleAuthorize>();
            ModuleApp = new SysModuleAppService();
            ModuleButtonApp = new SysButtonAppService();
        }

        public SysRoleAuthorizeAppService(IRepositoryBase<RoleAuthorize> roleAuthorizeRepository, SysModuleAppService moduleApp, SysButtonAppService moduleButtonApp)
        {
            Repository = roleAuthorizeRepository;
            ModuleApp = moduleApp;
            ModuleButtonApp = moduleButtonApp;
        }

        public List<RoleAuthorize> GetList(string ObjectId) => Repository.QueryAsNoTracking(t => t.F_ObjectId == ObjectId).ToList();

        public List<SysModule> GetMenuList(string roleId)
        {
            var data = new List<SysModule>();
            if (OperatorProvider.Current.IsSystem)
            {
                data = ModuleApp.GetList();
            }
            else
            {
                var moduledata = ModuleApp.GetList();
                var authorizedata = Repository.QueryAsNoTracking(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }

        public List<SysModule> GetEnableMenuList(string roleId)
        {
            var data = new List<SysModule>();
            if (OperatorProvider.Current.IsSystem)
            {
                data = ModuleApp.GetEnableList();
            }
            else
            {
                var moduledata = ModuleApp.GetEnableList();
                var authorizedata = Repository.QueryAsNoTracking(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }

        public List<SysButton> GetButtonList(string roleId)
        {
            var data = new List<SysButton>();
            if (OperatorProvider.Current.IsSystem)
            {
                data = ModuleButtonApp.GetList();
            }
            else
            {
                var buttondata = ModuleButtonApp.GetList();
                var authorizedata = Repository.QueryAsNoTracking(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.F_SortCode).ToList();
        }

        public bool ActionValidate(string roleId, string moduleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata = CacheFactory.Cache().GetCache<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = ModuleApp.GetList();
                var buttondata = ModuleButtonApp.GetList();
                var authorizedata = Repository.QueryAsNoTracking(t => t.F_ObjectId == roleId).ToList();
                foreach (var item in authorizedata)
                {
                    if (item.F_ItemType == 1)
                    {
                        var moduleEntity = moduledata.Find(t => t.F_Id == item.F_ItemId);
                        if (moduleEntity != null)
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.F_Id, F_UrlAddress = moduleEntity.F_UrlAddress });
                    }
                    else if (item.F_ItemType == 2)
                    {
                        var moduleButtonEntity = buttondata.Find(t => t.F_Id == item.F_ItemId);
                        if (moduleButtonEntity != null)
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.F_ModuleId, F_UrlAddress = moduleButtonEntity.F_UrlAddress });
                    }
                }
                CacheFactory.Cache().WriteCache(authorizeurldata, "authorizeurldata_" + roleId, DateTime.Now.AddMinutes(5));
            }
            else
            {
                authorizeurldata = cachedata;
            }
            authorizeurldata = authorizeurldata.FindAll(t => t.F_Id.Equals(moduleId));
            foreach (var item in authorizeurldata)
            {
                if (!string.IsNullOrEmpty(item.F_UrlAddress))
                {
                    var url = item.F_UrlAddress.Split('?');
                    if (item.F_Id == moduleId && url[0] == action)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}