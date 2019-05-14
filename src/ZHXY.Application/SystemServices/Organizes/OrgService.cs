using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using ZHXY.Common;

namespace ZHXY.Application
{

    /// <summary>
    /// 机构管理
    /// </summary>
    public class OrgService : AppService
    {
        public OrgService() => R = new ZhxyRepository();
        public OrgService(IZhxyRepository r) => R = r;
        public List<Organ> GetList()
        {
            return Read<Organ>().ToListAsync().Result;
        }

        public List<Organ> GetListById(string keyword = null)
        {
            var query = Read<Organ>();
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.Id.Contains(keyword));
            return query.OrderBy(t => t.SortCode).ToListAsync().Result;
        }
        /// <summary>
        /// 取出所有子机构
        /// </summary>
        public List<Organ> GetListByParentId(string parentId = null)
        {
            var list = new List<Organ>();
            var query = Read<Organ>();
            if (!string.IsNullOrEmpty(parentId))
            {
                list = query.Where(t => t.ParentId == parentId ).OrderBy(t => t.SortCode).ToListAsync().Result;
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
        public string GetClassInfosByDivisId(string divisId, ref List<Organ> classList)
        {
            var sysOrganizeList = Read<Organ>();
            var divis = sysOrganizeList.FirstOrDefault(p => p.Id.Equals(divisId));
            if (divis == null) throw new Exception("未找到该学部");
            var gradeList = sysOrganizeList.Where(p => p.ParentId.Equals(divisId)).Select(p => p.Id).ToList();
            classList = sysOrganizeList.Where(p => gradeList.Contains(p.ParentId)).ToList();
            return divis.Name;
        }
        public void GetClassInfosByGradeId(string gradeId, ref List<Organ> classList, ref List<Organ> divisList)
        {
            var sysOrganizeList = Read<Organ>();
            divisList = sysOrganizeList.Where(p => p.ParentId.Equals(gradeId)).ToList();
            var divisIds = divisList.Select(p => p.Id).ToList();
            classList = sysOrganizeList.Where(p => divisIds.Contains(p.ParentId)).ToList();
        }
        public dynamic GetById(string id) => Get<Organ>(id);

        public void Add(AddOrgDto dto)
        {
            var org = dto.MapTo<Organ>();
            AddAndSave(org);
        }

        public void Update(UpdateOrgDto dto)
        {
            var org = Get<Organ>(dto.Id);
            dto.MapTo(org);
            SaveChanges();
        }

        public List<Organ> GetListByOrgId(string orgIds)
        {
            var query = Read<Organ>();
            if (string.IsNullOrEmpty(orgIds))
            {
                var arrIds = orgIds.Split(',');
                query = query.Where(p => !arrIds.Contains(p.Id));
            }
            return query.ToListAsync().Result;
        }


        public void Delete(string id)
        {
            if (Read<Organ>(t => t.ParentId.Equals(id)).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            DelAndSave<Organ>(id);
        }


    }
}