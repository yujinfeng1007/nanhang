using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{
    public class RoleAuthorizeService : AppService
    {
        public RoleAuthorizeService(DbContext r) : base(r)
        {
        }

        public List<RoleAuthorize> GetList(string ObjectId) =>Read<RoleAuthorize>(t => t.ObjectId == ObjectId).ToList();

        public List<Module> GetMenuList(string roleId,string clientType)
        {
            var data = new List<Module>();
            if (Operator.GetCurrent().IsSystem)
            {
                data = Read<Module>().ToList();
            }
            else
            {
                var moduledata = Read<Module>(t=>t.BelongSys== clientType).ToList();
                var authorizedata = Read<RoleAuthorize>(t => t.ObjectId == roleId && t.ItemType == 1).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleEntity = moduledata.Find(t => t.Id == item.ItemId);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.Sort).ToList();
        }

        public List<Button> GetButtonList(string roleId)
        {
            var data = new List<Button>();
            if (Operator.GetCurrent().IsSystem)
            {
                data = Read<Button>().ToList();
            }
            else
            {
                var buttondata = Read<Button>().ToList();
                var authorizedata = Read<RoleAuthorize>(t => t.ObjectId == roleId && t.ItemType == 2).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleButtonEntity = buttondata.Find(t => t.Id == item.ItemId);
                    if (moduleButtonEntity != null)
                    {
                        data.Add(moduleButtonEntity);
                    }
                }
            }
            return data.OrderBy(t => t.Sort).ToList();
        }

        public bool ActionValidate(string roleId, string moduleId, string action)
        {
            var authorizeurldata = new List<AuthorizeActionModel>();
            var cachedata = RedisCache.Get<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = Read<Module>().ToList();
                var buttondata = Read<Button>().ToList();
                var authorizedata =Read<RoleAuthorize>(t => t.ObjectId == roleId).ToList();
                foreach (var item in authorizedata)
                {
                    if (item.ItemType == 1)
                    {
                        var moduleEntity = moduledata.Find(t => t.Id == item.ItemId);
                        if (moduleEntity != null)
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleEntity.Id, F_UrlAddress = moduleEntity.Url });
                    }
                    else if (item.ItemType == 2)
                    {
                        var moduleButtonEntity = buttondata.Find(t => t.Id == item.ItemId);
                        if (moduleButtonEntity != null)
                            authorizeurldata.Add(new AuthorizeActionModel { F_Id = moduleButtonEntity.ModuleId, F_UrlAddress = moduleButtonEntity.Url });
                    }
                }
                RedisCache.Set("authorizeurldata_" + roleId, authorizeurldata, DateTime.Now.AddMinutes(5));
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