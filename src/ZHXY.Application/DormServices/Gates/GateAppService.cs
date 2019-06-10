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
        public GateAppService(DbContext r) : base(r) { }
        public List<GateView> GetList(Pagination pag, string keyword)
        {
            var query = Read<Gate>();
            query = string.IsNullOrWhiteSpace(keyword) ? query : query.Where(p => p.DeviceNumber.Contains(keyword) || p.Name.Contains(keyword) || p.Location.Contains(keyword));
            pag.Records = query.Count();
            var list = query.Paging(pag).ToList();
            return list.MapToList<GateView>();
        }

        public List<Gate> GetList(){
            return Query<Gate>().ToList();
        }

        public void SetStatus(string[] ids, int status)
        {
            var gates = Query<Gate>(p => ids.Contains(p.Id)).ToList();
            gates.ForEach(item => item.Status = status);
            foreach(var item in gates)
            {
                item.Status = status;
                DHAccount.OpenDoor(status, item.DeviceNumber);
            }
           
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
        public void Sync(AddGateDto input)
        {
            var gate = Read<Gate>(t=>t.DeviceNumber==input.DeviceNumber);
            if (gate != null)
            {
                var entity = input.MapTo<Gate>();
                AddAndSave(entity);
            }
        }

        public void BindBuilding(string id, string[] buildings)
        {
            foreach (var item in buildings)
            {

                var rel = new Relevance
                {
                    Name = Relation.GateBuilding,
                    FirstKey = id,
                    SecondKey = item
                };
                AddAndSave(rel);
            }
            SaveChanges();

        }

        public void UnbindBuilding(string id, string buildingId)
        {
            var obj = Query<Relevance>(p => p.Name.Equals(Relation.GateBuilding) && p.FirstKey.Equals(id) && p.SecondKey.Equals(buildingId)).FirstOrDefault();
            DelAndSave(obj);
        }

        /// <summary>
        /// 获取闸机绑定的楼栋
        /// </summary>
        /// <param name="id"></param>
        public List<Building> GetBoundBuildings(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(Relation.GateBuilding) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<Building>(p => buildingIds.Contains(p.Id)).ToList();
            return list;
        }

        public List<Building> GetBuildings()
        {
            return Read<Building>().ToList();
        }


        public List<Building> GetNotBoundBuildings(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(Relation.GateBuilding) && p.FirstKey.Equals(id)).Select(p => p.SecondKey).ToArray();
            var list = Read<Building>(p => !buildingIds.Contains(p.Id)).OrderBy(t => t.BuildingNo).ToList();
            return list;
        }

        /// <summary>
        /// 获取闸机的用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public dynamic GetUsers(string id)
        {
            var builidingIds = Read<Relevance>(p => p.Name == Relation.GateBuilding && p.FirstKey == id).Select(p => p.SecondKey).ToList();

            var userIds = Read<Relevance>(p => p.Name == Relation.BuildingUser && builidingIds.Contains(p.FirstKey)).Select(t => t.SecondKey).ToList();

            return Read<User>(p=>userIds.Contains(p.Id)).OrderBy(p => p.Name).Take(10).Select(p => new
            {
                id = p.Id,
                name = p.Name
            }).ToListAsync().Result;
        }

        public dynamic GetByBuilding(string id)
        {
            var buildingIds = Read<Relevance>(p => p.Name.Equals(Relation.GateBuilding) && p.FirstKey.Equals(id)).Select(p => p.FirstKey).ToArray();
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
