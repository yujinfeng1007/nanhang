using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Application
{

    /// <summary>
    /// 机构管理
    /// </summary>
    public class OrgService : AppService
    {
        public OrgService() => R = new EFContext();
        public OrgService(DbContext r) => R = r;
        public List<Org> GetList()
        {
            return Read<Org>().ToListAsync().Result;
        }

        public List<Org> GetListById(string keyword = null)
        {
            var query = Read<Org>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.Id.Contains(keyword));
            return query.OrderBy(t => t.Sort).ToListAsync().Result;
        }
        /// <summary>
        /// 取出所有子机构
        /// </summary>
        public List<Org> GetListByParentId(string parentId = null)
        {
            var list = new List<Org>();
            var query = Read<Org>();
            if (!string.IsNullOrEmpty(parentId))
            {
                list = query.Where(t => t.ParentId == parentId ).OrderBy(t => t.Sort).ToListAsync().Result;
                if (true)
                {
                    foreach (var org in list)
                    {
                        list = list.Concat(GetListByParentId(org.Id)).ToList();
                    }
                }
            }
            return list;
        }
        public string GetClassInfosByDivisId(string divisId, ref List<Org> classList)
        {
            var sysOrganizeList = Read<Org>();
            var divis = sysOrganizeList.FirstOrDefault(p => p.Id.Equals(divisId));
            if (divis == null) throw new Exception("未找到该学部");
            var gradeList = sysOrganizeList.Where(p => p.ParentId.Equals(divisId)).Select(p => p.Id).ToList();
            classList = sysOrganizeList.Where(p => gradeList.Contains(p.ParentId)).ToList();
            return divis.Name;
        }
        public void GetClassInfosByGradeId(string gradeId, ref List<Org> classList, ref List<Org> divisList)
        {
            var sysOrganizeList = Read<Org>();
            divisList = sysOrganizeList.Where(p => p.ParentId.Equals(gradeId)).ToList();
            var divisIds = divisList.Select(p => p.Id).ToList();
            classList = sysOrganizeList.Where(p => divisIds.Contains(p.ParentId)).ToList();
        }
        public Org GetById(string id) => Get<Org>(id);

        public void Add(AddOrgDto dto)
        {
            var org = dto.MapTo<Org>();
            AddAndSave(org);
        }

        public void Update(UpdateOrgDto dto)
        {
            var org = Get<Org>(dto.Id);
            dto.MapTo(org);
            SaveChanges();
        }

        public List<Org> GetListByOrgId(string orgIds)
        {
            var query = Read<Org>();
            if (string.IsNullOrEmpty(orgIds))
            {
                var arrIds = orgIds.Split(',');
                query = query.Where(p => !arrIds.Contains(p.Id));
            }
            return query.ToListAsync().Result;
        }


        public void Delete(string id)
        {
            if (Read<Org>(t => t.ParentId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            DelAndSave<Org>(id);
        }


    }
}