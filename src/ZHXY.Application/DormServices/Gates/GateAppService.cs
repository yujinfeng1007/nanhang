using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using ZHXY.Common;
using ZHXY.Domain;

namespace ZHXY.Application
{

    /// <summary>
    /// 闸机管理
    /// </summary>
    public class GateAppService : AppService
    {
        public GateAppService(IZhxyRepository r) : base(r) { }
        public List<GateView> GetList(Pagination pag, string keyword)
        {
            var query = Read<Gate>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.DeviceNumber.Contains(keyword) || p.Name.Contains(keyword) || p.Location.Contains(keyword));
            pag.Records = query.Count();
            var list = query.Paging(pag).ToList();
            return list.MapToList<GateView>();
        }

        public void SetStatus(string[] ids, int status)
        {
            var gates = Query<Gate>(p => ids.Contains(p.Id)).ToList();
            gates.ForEach(item => item.Status = status);
            SaveChanges();
        }

        public GateView GetById(string id)
        {
            var gate = Get<Gate>(id);
            return gate.MapTo<GateView>();
        }

        public void Add(AddGateDto input)
        {
            var gate = input.MapTo<Gate>();
            AddAndSave(gate);
        }

        public void Update(UpdateGateDto input)
        {
            var gate = Get<Gate>(input.Id);
            input.MapTo(gate);
            SaveChanges();
        }
        public void Sync(SyncGateDto input)
        {
            var gate = Get<Gate>(input.Id);
            input.MapTo(gate);
            SaveChanges();
        }

        public void BindBuilding(string id, string[] buildings)
        {
            foreach (var item in buildings)
            {

                var rel = new Relevance
                {
                    Name = SYS_CONSTS.REL_GATE_BUILDING,
                    FirstKey = id,
                    SecondKey = item
                };
                Add(rel);
            }
            SaveChanges();

        }

        public void UnbindBuilding(string id, string buildingId)
        {
            var obj = Query<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_GATE_BUILDING) && p.FirstKey.Equals(id) && p.SecondKey.Equals(buildingId)).FirstOrDefault();
            DelAndSave(obj);
        }

        /// <summary>
        /// 获取闸机绑定的楼栋
        /// </summary>
        /// <param name="id"></param>
        public List<Building> GetBoundBuildings(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_GATE_BUILDING) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<Building>(p => buildingIds.Contains(p.Id)).ToList();
            return list;
        }


        public List<Building> GetNotBoundBuildings(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_GATE_BUILDING) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<Building>(p => !buildingIds.Contains(p.Id)).ToList();
            return list;
        }

        /// <summary>
        /// 获取闸机的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetUsers(string id)
        {
            return Read<User>().OrderBy(p => p.F_NickName).Take(10).Select(p => new
            {
                id = p.F_Id,
                name = p.F_RealName
            }).ToListAsync().Result;
        }

        public dynamic GetByBuilding(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(SYS_CONSTS.REL_GATE_BUILDING) && p.FirstKey.Equals(id)).Select(p => p.FirstKey).ToArray();
            var list = Read<Gate>(p => !buildingIds.Contains(p.Id)).Select(p =>
            new
            {
                id = p.Id,
                name = p.Name
            }).ToList();
            return list;
        }
    }
}
