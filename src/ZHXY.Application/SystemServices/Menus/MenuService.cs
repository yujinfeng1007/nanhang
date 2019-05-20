using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using ZHXY.Common;
using System.Data.Entity;
namespace ZHXY.Application
{
    /// <summary>
    /// 菜单管理
    /// </summary>
    public class MenuService : AppService
    {
        public MenuService(IZhxyRepository r) : base(r) { }

        public void Add(AddMenuDto dto)
        {
            var menu = dto.MapTo<Menu>();
            SetParentAndFullName(menu);
            AddAndSave(menu);
        }

        public void Delete(string id)
        {
            if (Read<Menu>(p => p.ParentId.Equals(id)).Any()) throw new Exception("包含下级,不能删除!");
            DelAndSave<Menu>(id);
        }

        public void Update(UpdateMenuDto dto)
        {
            var menu = Get<Menu>(dto.Id);
            dto.MapTo(menu);
            SetParentAndFullName(menu);
            SaveChanges();
        }
        private void SetParentAndFullName(Menu menu)
        {
            menu.ParentId = string.IsNullOrWhiteSpace(menu.ParentId) ? SYS_CONSTS.DbNull :
                !Read<Menu>(p => p.ParentId.Equals(menu.ParentId)).Any() ? SYS_CONSTS.DbNull :
                menu.ParentId;
            if (menu.ParentId.Equals(SYS_CONSTS.DbNull))
            {
                menu.FullName = menu.Name;
            }
            else
            {
                var parent = Read<Menu>(p => p.Id.Equals(menu.ParentId)).FirstOrDefaultAsync().Result;
                menu.FullName = $"{parent.FullName}/{menu.Name}";
                menu.Level = parent.Level + 1;
            }
        }


        public List<MenuView> GetMenu(string parentId, int level = 0)
        {
            var list = new List<MenuView>();
            level = string.IsNullOrWhiteSpace(parentId) ? 0 : level + 1;
            parentId = string.IsNullOrWhiteSpace(parentId) ? SYS_CONSTS.DbNull : parentId;
            Read<Menu>(p => p.ParentId.Equals(parentId)).OrderBy(p => p.SortCode).ToListAsync().Result.ForEach(p =>
            {
                var item = p.MapTo<MenuView>();
                item.Level = level;
                item.IsLeaf = !Read<Menu>(m => m.ParentId.Equals(p.Id)).Any();
                list.Add(item);
            });
            return list;
        }

        public void DeleteFunc(string[] id)
        {
            var btns = Query<Function>(p => id.Contains(p.Id)).ToListAsync().Result;
            DelAndSave<Function>(btns);
        }

        public void AddFunc(AddFuncDto dto)
        {
            var button = dto.MapTo<Function>();
            AddAndSave(button);
        }

        public void UpdateFunc(UpdateFuncDto dto)
        {
            var button = Get<Function>(dto.Id);
            dto.MapTo(button);
            SaveChanges();
        }

        public dynamic GetMenuFunc(string menuId)
        {
            return Read<Function>(p => p.MenuId.Equals(menuId)).OrderBy(p => p.SortCode).ToListAsync().Result;
        }

        /// <summary>
        /// 获取所有子菜单(递归获取)
        /// </summary>
        /// <param name="pid">根节点id</param>
        /// <returns></returns>
        private IEnumerable<Menu> GetChildren(string pid = null)
        {
            pid = string.IsNullOrWhiteSpace(pid) ? SYS_CONSTS.DbNull : pid;
            var query = Read<Menu>(p => p.ParentId.Equals(pid));
            return query.ToList().Concat(query.ToList().SelectMany(p => GetChildren(p.Id)));
        }
    }
}