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
            await SaveChangesAsync();
            return entity;
        }

        public async Task DelAsync(string id) => await Task.Run(() => DelAndSave<Building>(id));

        public async Task<DormBuildingView> UpdAsync(UpdateDormBuildingDto input)
        {
            if (string.IsNullOrEmpty(input.Id)) throw new ArgumentNullException(nameof(input.Id));
            var entity =  Get<Building>(input.Id);
            if (entity == null) throw new Exception($"No objects were found based on this Id : {input.Id}");
            input.MapTo(entity);            
            await SaveChangesAsync();
            return entity;
        }

        public DormBuildingView Get(string id) => GetAsync<Building>(id).Result;

        public List<DormBuildingView> GetList(Pagination pagination, string keyword)
        {
            var query = Read<Building>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(p => p.BuildingNo.Contains(keyword));
            pagination.Records = query.CountAsync().Result;
            pagination.GetOrdering<Building>();
            query = string.Equals("false", pagination.Sidx, StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(p => p.BuildingNo) : query.Paging(pagination);
            query = query.Skip(pagination.Skip).Take(pagination.Rows);
            return query.ToListAsync().Result.MapToList<DormBuildingView>();
        }

        /// <summary>
        /// 获取所有楼栋
        /// auth:yujinfeng
        /// </summary>
        /// <returns></returns>
        public dynamic GetAll()
        {
            return Read<Building>().Select(p => new
            {
                id=p.Id,
                name=p.BuildingNo
            }).ToListAsync().Result;
        }


        /// <summary>
        /// 绑定楼栋的宿管
        /// </summary>
        public void BindUsers(string id, string[] users) {
            foreach (var user in users) {
                var rel = new Relevance
                {
                    Name = Relation.BuildingUser,
                    FirstKey = id,
                    SecondKey = user
                };
                AddAndSave(rel);
            }
            SaveChanges();
        }

      
        /// <summary>
        /// 解除楼栋所绑定的宿管
        /// </summary>
        public void UnBindUser(string id, string userId)
        {
            var user = Query<Relevance>(p => p.Name.Equals(Relation.BuildingUser) && p.FirstKey.Equals(id) && p.SecondKey.Equals(userId)).FirstOrDefault();
            DelAndSave(user);
        }


        /// <summary>
        /// 获取楼栋所绑定的宿管
        /// </summary>        
        public List<User> GetSubBindUsers(string id)
        {
            var usersIds = Read<Relevance>(p => p.Name.Equals(Relation.BuildingUser) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<User>(p => usersIds.Contains(p.Id)).ToList();
            return list;
        }

        /// <summary>
        /// 获取楼栋未绑定的宿管 宿管机构ID:27e1854fd963d24b8c5d88506a775c2a  或者根据角色宿管进行过滤
        /// </summary>        
        public List<User> GetNotBindUsers(string id)
        {
            var usersIds = Read<Relevance>(p => p.Name.Equals(Relation.BuildingUser) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<User>(p => !usersIds.Contains(p.Id) && p.OrganId == "EEA8D099CF7F462888473FE0937A0F6D").ToList();
            //var list = Read<User>(p => !usersIds.Contains(p.Id) && p.DutyId.Contains("dormitory") ).ToList();  //后续更新为通过岗位来区分宿管
            return list;
        }

        /// <summary>
        /// 根据闸机设备号获取绑定的楼栋列表
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <returns></returns>
        public List<Building> GetBindGate(string deviceNumber)
        {
            var relevances = Read<Relevance>(p => p.FirstKey == deviceNumber && p.Name == "Gate_Building").Select(p => p.SecondKey).ToList();

            var list = Read<Building>(p => relevances.Contains(p.Id)).ToList();

            return list;
        }

        /// <summary>
        /// 闸机绑定楼栋信息
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <param name="buildingId"></param>
        public void BindBuildings(string deviceNumber,string buildingId)
        {
            var relevance = new Relevance();
            relevance.Name = "Gate_Building";
            relevance.FirstKey = deviceNumber;
            relevance.SecondKey = buildingId;

            AddAndSave(relevance);
        }

        /// <summary>
        /// 闸机解绑楼栋信息
        /// </summary>
        /// <param name="deviceNumber"></param>
        public void UnBindBuildings(string deviceNumber)
        {
            var relevance = Read<Relevance>(p => p.Name == "Gate_Building" && p.FirstKey == deviceNumber).FirstOrDefault();

            if (relevance != null)
            {
                DelAndSave(relevance);
            }
        }


        /// <summary>
        /// 获取楼栋学生基本外出在寝信息
        /// </summary>
        /// <param name="deviceNumber"></param>
        /// <returns></returns>
        public dynamic GetDormOutInInfo(string deviceNumber)
        {
            var buildingIds = Read<Relevance>(p => p.Name == "Gate_Building" && p.FirstKey==deviceNumber).Select(p => p.FirstKey).ToList();

            var buildingNos = Read<Gate>(p => buildingIds.Contains(p.Id)).Select(p=>p.DeviceNumber).ToList();

            var outs = Read<LDJCLS>(p => buildingNos.Contains(p.BuildingNo) && p.Direction == true)
                .GroupBy(p => new { p.BuildingNo }).Select(p => new {
                    F_OutNum = p.Count(),
                    p.Key.BuildingNo,
                });

            var ins = Read<LDJCLS>(p => buildingNos.Contains(p.BuildingNo) && p.Direction == false)
                .GroupBy(p => new { p.BuildingNo }).Select(p => new {
                    F_InNum = p.Count(),
                    p.Key.BuildingNo,
                });

           return  outs.Join(ins, e => e.BuildingNo, o => o.BuildingNo, (e, o) => new { e.BuildingNo, e.F_OutNum,o.F_InNum}).ToList();
           // var data = outs.GroupJoin(ins, a => new { a.building_no, a.F_OutNum }, b => new { b.building_no, b.F_InNum }, (a, b) => new {  a.building_no, a.F_OutNum, b });
        }

    }
}