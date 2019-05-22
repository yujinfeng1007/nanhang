using System.Collections.Generic;
using System.Linq;

using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{
    public class RoleAuthorizeService : AppService
    {

        public RoleAuthorizeService(IZhxyRepository r) : base(r)
        {
        }
        public List<Menu> GetEnableMenuList(string roleId)
        {
            var data = new List<Menu>();
            var menus = Read<Menu>().OrderBy(t => t.SortCode).ToList();
            if (Operator.GetCurrent().IsSystem)
            {
                data = menus;
            }
            else
            {
                var moduledata = menus;
                var authorizedata = Query<Relevance>(p => p.Name.Equals(Relation.RolePower) && p.FirstKey.Equals(roleId)).ToList();
                foreach (var item in authorizedata)
                {
                    var moduleEntity = moduledata.Find(t => t.Id == item.SecondKey);
                    if (moduleEntity != null)
                    {
                        data.Add(moduleEntity);
                    }
                }
            }
            return data.OrderBy(t => t.SortCode).ToList();
        }

      
    }
}