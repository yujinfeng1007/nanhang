using System;
using System.Data.Entity;
using System.Linq;
using ZHXY.Domain;

namespace ZHXY.Application
{
    /// <summary>
    /// 机构负责人管理
    /// </summary>
    public class OrgLeaderAppService : AppService
    {
        public OrgLeaderAppService(IZhxyRepository r) => R = r;

        public dynamic Get( string orgId)
        {
            return Read<OrgLeader>(p => p.OrgId.Equals(orgId)).Select(p => new { orgId = p.OrgId, orgName = p.Org.Name, userId = p.UserId, userName = p.User.Name }).ToListAsync().Result;
        }

        /// <summary>
        /// 添加负责人
        /// </summary>
        public void Add( AddOrRemoveOrgLeaderDto input)
        {
            var already = Read<OrgLeader>(p => p.OrgId.Equals(input.OrgId)).Select(p => p.UserId).ToArrayAsync().Result;
            var users= input.Users.Except(already);
            foreach (var item in users)
            {
                Add(new OrgLeader { UserId = item, OrgId = input.OrgId });
            }
            SaveChanges();
        }

        /// <summary>
        /// 移除负责人
        /// </summary>
        public void Remove(  AddOrRemoveOrgLeaderDto input) => DelAndSave<OrgLeader>(p => p.OrgId.Equals(input.OrgId) && input.Users.Contains(p.UserId));

                      
    }


}