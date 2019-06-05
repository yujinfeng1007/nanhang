using ZHXY.Domain;

namespace ZHXY.Application
{
    public class RelevanceService : AppService
    {
        public RelevanceService(IZhxyRepository r) : base(r) { }

        ///// <summary>
        ///// 获取用户角色
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public string[] GetUserRole(string userId)
        //{
        //    return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.FirstKey.Equals(userId)).Select(p => p.SecondKey).ToArrayAsync().Result;
        //}

        ///// <summary>
        ///// 获取角色用户
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <returns></returns>
        //public string[] GetRoleUser(string roleId)
        //{
        //    return Read<Relevance>(p => p.Name.Equals(Relation.UserRole) && p.SecondKey.Equals(roleId)).Select(p => p.FirstKey).ToArrayAsync().Result;
        //}

        ///// <summary>
        ///// 添加角色资源
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <param name="resources"></param>
        //public void AddRoleResource(string roleId, string[] resources)
        //{
        //    var items = resources.Except(GetRoleResource(roleId));
        //    foreach (var item in items)
        //    {
        //        Add(new Relevance { Name = Relation.RoleResource, FirstKey = roleId, SecondKey = item });
        //    }
        //    SaveChanges();
        //}

        ///// <summary>
        ///// 获取角色资源
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <returns></returns>
        //public string[] GetRoleResource(string roleId)
        //{
        //    return Read<Relevance>(p => p.Name.Equals(Relation.RoleResource) && p.FirstKey.Equals(roleId)).Select(p => p.SecondKey).ToArrayAsync().Result;
        //}

        ///// <summary>
        ///// 获取用户资源
        ///// </summary>
        ///// <param name="userId"></param>
        ///// <returns></returns>
        //public string[] GetUserRosource(string userId)
        //{
        //    var userRoles = GetUserRole(userId);
        //    return Read<Relevance>(p => p.Name.Equals(Relation.RoleResource) && userRoles.Contains(p.FirstKey)).Select(p => p.SecondKey).ToArrayAsync().Result;
        //}
    }
}
