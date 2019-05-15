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
            menu.ParentId = string.IsNullOrWhiteSpace(menu.ParentId) ? "0": menu.ParentId;
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

        public dynamic GetMenu(string nodeId=null, int nodeLevel = 0)
        {
            nodeLevel = string.IsNullOrWhiteSpace(nodeId) ? 0 : nodeLevel + 1;
            nodeId = string.IsNullOrWhiteSpace(nodeId) ? "0" : nodeId;
            return Read<Menu>(p=>p.ParentId.Equals(nodeId)).Select(p=>
                new MenuView
                {
                    Id=p.Id,
                    ParentId = p.ParentId,
                    Level = nodeLevel,
                    EnCode = p.EnCode,
                    Name = p.Name,
                    Icon = p.Icon,
                    IconForWeb = p.IconForWeb,
                    Url = p.Url,
                    Target = p.Target,
                    IsMenu = p.IsMenu,
                    IsExpand = p.IsExpand,
                    IsPublic = p.IsPublic,
                    SortCode = p.SortCode,
                    BelongSys = p.BelongSys,
                    IsLeaf = !p.ChildNodes.Any(),
                } ).ToListAsync().Result;
        }

        public void DeleteBtn(string[] id)
        {
            var btns=Query<Button>(p => id.Contains(p.Id)).ToListAsync().Result;
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
            return Read<Button>(p => p.MenuId.Equals(menuId)).ToListAsync().Result;
        }

        public List<MenuView> GetMenuTree()
        {
            // with temp as
            //(
            //    select *, 0 as cur_level from sys_module where f_parentid = '0'
            //    union all
            //    select a.*, b.cur_level + 1 from sys_module a
            //    inner join temp b on (a.f_parentid = b.f_id)
            //)
            //select* from temp; 
            return GetChildren().MapToList<MenuView>();
        }

        private IEnumerable<Menu> GetChildren(string pid="0")
        {
            var query = Read<Menu>(p => p.ParentId.Equals(pid));
            return query.ToList().Concat(query.ToList().SelectMany(p => GetChildren(p.Id)));
        }
    }
}