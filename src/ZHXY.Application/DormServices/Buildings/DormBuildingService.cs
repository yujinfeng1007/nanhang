using ZHXY.Common;
using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;

namespace ZHXY.Application
{
    /// <summary>
    /// 楼栋管理
    /// author: zby
    /// phone:  
    /// email:  
    /// </summary>
    public class DormBuildingService : AppService
    {
        public DormBuildingService(IZhxyRepository r) : base(r) { }

        public async Task<DormBuildingView> AddAsync(CreateDormBuildingDto input)
        {
            Building entity = input;            
            AddAndSave(entity);
            return entity;
        }

        public async Task DelAsync(string id) => await Task.Run(() => DelAndSave<Building>(id));

        public async Task<DormBuildingView> UpdAsync(UpdateDormBuildingDto input)
        {
            if (string.IsNullOrEmpty(input.Id)) throw new ArgumentNullException(nameof(input.Id));
            var entity =  Get<Building>(input.Id);
            if (entity == null) throw new Exception($"No objects were found based on this Id : {input.Id}");
            input.MapTo(entity);            
             SaveChanges();
            return entity;
        }

        public DormBuildingView Get(string id) => GetAsync<Building>(id).Result;

        public List<DormBuildingView> GetList(Pagination pagination, string keyword)
        {
            var query = Read<Building>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.Title.Contains(keyword));
            pagination.Records = query.CountAsync().Result;
            pagination.CheckSort<Building>();
            query = string.Equals("false", pagination.Sidx, StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(p => p.BuildingNo) : query.OrderBy(pagination.Sidx);
            query = query.Skip(pagination.Skip).Take(pagination.Rows);
            return query.ToListAsync().Result.MapToList<DormBuildingView>();
        }
       

        /// <summary>
        /// 绑定楼栋的宿管
        /// </summary>
        public void BindUsers(string id, string[] users) {
            foreach (var user in users) {
                var rel = new Relevance
                {
                    Name = SYS_CONSTS.REL_BUILDING_USERS,
                    FirstKey = id,
                    SecondKey = user
                };
                Add(rel);
            }
            SaveChanges();
        }

      
        /// <summary>
        /// 解除楼栋所绑定的宿管
        /// </summary>
        public void UnBindUser(string id, string userId)
        {
            var user = Query<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_BUILDING_USERS) && p.FirstKey.Equals(id) && p.SecondKey.Equals(userId)).FirstOrDefault();
            DelAndSave(user);
        }


        /// <summary>
        /// 获取楼栋所绑定的宿管
        /// </summary>        
        public List<User> GetSubBindUsers(string id)
        {
            var usersIds = Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_BUILDING_USERS) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<User>(p => usersIds.Contains(p.F_Id)).ToList();
            return list;
        }

        /// <summary>
        /// 获取楼栋未绑定的宿管 宿管机构ID:27e1854fd963d24b8c5d88506a775c2a  或者根据角色宿管进行过滤
        /// </summary>        
        public List<User> GetNotBindUsers(string id)
        {
            var usersIds = Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_BUILDING_USERS) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<User>(p => !usersIds.Contains(p.F_Id) && p.F_OrganizeId == "27e1854fd963d24b8c5d88506a775c2a").ToList();
            return list;
        }
       

    }
}