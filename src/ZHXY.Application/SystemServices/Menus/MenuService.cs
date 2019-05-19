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
            menu.ParentId = string.IsNullOrWhiteSpace(menu.ParentId) ? SYS_CONSTS.DbNull :
                !Read<Menu>(p => p.ParentId.Equals(dto.ParentId)).Any() ? SYS_CONSTS.DbNull :
                menu.ParentId;
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
            SaveChanges();
        }

        public dynamic GetMenu(string parentId, int level = 0)
        {
            level = string.IsNullOrWhiteSpace(parentId) ? 0 : level + 1;
            parentId = string.IsNullOrWhiteSpace(parentId) ? SYS_CONSTS.DbNull : parentId;
            return Read<Menu>(p => p.ParentId.Equals(parentId)).OrderBy(p => p.SortCode).Select(p =>
                    new
                    {
                        p.Id,
                        p.ParentId,
                        p.Name,
                        p.Icon,
                        p.IconForWeb,
                        p.Url,
                        p.Target,
                        p.IsMenu,
                        p.IsExpand,
                        p.IsPublic,
                        p.SortCode,
                        p.BelongSys,
                        level,
                        IsLeaf = !p.ChildNodes.Any(),
                    }).ToListAsync().Result;
        }

        public void DeleteBtn(string[] id)
        {
            var btns = Query<Button>(p => id.Contains(p.Id)).ToListAsync().Result;
            DelAndSave<Button>(btns);
        }

        public void AddBtn(AddBtnDto dto)
        {
            var button = dto.MapTo<Button>();
            AddAndSave(button);
        }

        public void UpdateBtn(UpdateBtnDto dto)
        {
            var button = Get<Button>(dto.Id);
            dto.MapTo(button);
            SaveChanges();
        }

        public dynamic GetMenuBth(string menuId)
        {
            return Read<Button>(p => p.MenuId.Equals(menuId)).OrderBy(p=>p.SortCode).ToListAsync().Result;
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