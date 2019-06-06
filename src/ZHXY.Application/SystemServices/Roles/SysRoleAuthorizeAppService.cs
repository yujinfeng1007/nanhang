using System;
using System.Collections.Generic;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain;
namespace ZHXY.Application
{
    public class SysRoleAuthorizeAppService : AppService
    {
        public SysRoleAuthorizeAppService(IZhxyRepository r) : base(r)
        {
        }

        public List<SysRoleAuthorize> GetList(string ObjectId) =>Read<SysRoleAuthorize>(t => t.F_ObjectId == ObjectId).ToList();

        public List<SysModule> GetMenuList(string roleId,string clientType)
        {
            var data = new List<SysModule>();
            if (Operator.GetCurrent().IsSystem)
            {
                data = Read<SysModule>().ToList();
            }
            else
            {
                var moduledata = Read<SysModule>(t=>t.F_BelongSys== clientType).ToList();
                var authorizedata = Read<SysRoleAuthorize>(t => t.F_ObjectId == roleId && t.F_ItemType == 1).ToList();
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
            if (Operator.GetCurrent().IsSystem)
            {
                data = Read<SysButton>().ToList();
            }
            else
            {
                var buttondata = Read<SysButton>().ToList();
                var authorizedata = Read<SysRoleAuthorize>(t => t.F_ObjectId == roleId && t.F_ItemType == 2).ToList();
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
            var cachedata = RedisCache.Get<List<AuthorizeActionModel>>("authorizeurldata_" + roleId);
            if (cachedata == null)
            {
                var moduledata = Read<SysModule>().ToList();
                var buttondata = Read<SysButton>().ToList();
                var authorizedata =Read<SysRoleAuthorize>(t => t.F_ObjectId == roleId).ToList();
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

    public class AuthorizeActionModel
    {
        public string F_Id { set; get; }
        public string F_UrlAddress { set; get; }
    }
}