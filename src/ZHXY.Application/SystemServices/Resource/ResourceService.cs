using System;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 资源服务
    /// </summary>
    public class ResourceService : AppService
    {
        public RelevanceService RelevanceApp { get; }

        public ResourceService(IZhxyRepository r) : base(r)
        {
        }

        public ResourceService(IZhxyRepository r, RelevanceService relevanceApp) : base(r)
        {
            RelevanceApp = relevanceApp;
        }


        public void AddMenu(AddMenuDto dto)
        {
            var menu = dto.MapTo<Resource>();
            SetParentAndFullName(menu);
            AddAndSave(menu);
        }


        public void UpdateMenu(UpdateMenuDto dto)
        {
            var menu = Get<Resource>(dto.Id);
            dto.MapTo(menu);
            SetParentAndFullName(menu);
            SaveChanges();
        }

        public void AddFunc(AddFuncDto dto)
        {
            var menu = dto.MapTo<Resource>();
            SetParentAndFullName(menu);
            AddAndSave(menu);
        }

        public void UpdateFunc(UpdateFuncDto dto)
        {
            var menu = Get<Resource>(dto.Id);
            dto.MapTo(menu);
            SetParentAndFullName(menu);
            SaveChanges();
        }

        public void Delete(string id)
        {
            if (Read<Resource>(p => p.ParentId.Equals(id)).Any()) throw new Exception("包含下级,不能删除!");
            DelAndSave<Resource>(id);
        }

        private void SetParentAndFullName(Resource resource)
        {
            resource.ParentId = string.IsNullOrWhiteSpace(resource.ParentId) ? SYS_CONSTS.DbNull :
                !Read<Resource>(p => p.ParentId.Equals(resource.ParentId)).Any() ? SYS_CONSTS.DbNull :
                resource.ParentId;
            if (resource.ParentId.Equals(SYS_CONSTS.DbNull))
            {
                resource.FullName = resource.Name;
                resource.Level = 0;
            }
            else
            {
                var parent = Read<Menu>(p => p.Id.Equals(resource.ParentId)).FirstOrDefaultAsync().Result;
                resource.FullName = $"{parent.FullName}/{resource.Name}";
                resource.Level = parent.Level + 1;
            }
        }

        public dynamic GetUserMenu(string userId)
        {
            var userMenus = RelevanceApp.GetUserRosource(userId);
            return Read<Resource>(p => userMenus.Contains(p.Id) && p.Type.Equals(SYS_CONSTS.Menu)).ToListAsync().Result;
        }

        public dynamic GetAllMenu()
        {
            var list = Read<Resource>(p => p.Type.Equals(SYS_CONSTS.Menu)).OrderBy(p => p.FullName).ToListAsync().Result;
            return list.Select(p => new
                {
                    p.Id,
                    p.ParentId,
                    p.Level,
                    p.Name,
                    p.Type,
                    p.FullName,
                    p.BelongSys,
                    p.Icon,
                    p.IconForWeb,
                    IsLeaf = !Read<Resource>(c => c.ParentId.Equals(p.Id) && p.Type.Equals(SYS_CONSTS.Menu)).Any(),
                    Loaded = true,
                    Expanded = false
                }).ToList();
        }
    }
}