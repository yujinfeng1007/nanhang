using ZHXY.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using ZHXY.Common;

namespace ZHXY.Application
{

    /// <summary>
    /// 机构管理
    /// </summary>
    public class SysOrganizeAppService : AppService
    {
        public SysOrganizeAppService() => R = new ZhxyRepository();

        public SysOrganizeAppService(IZhxyRepository r) => R = r;


        public List<Organize> GetList()
        {
            return Read<Organize>(t => t.F_DeleteMark != true).OrderBy(t => t.F_CreatorTime).ToListAsync().Result;
        }

        public List<Organize> GetList(Expression<Func<Organize, bool>> expr = null)
        {
            return Read(expr).ToListAsync().Result;
        }

        public List<Organize> GetListById(string keyword = null)
        {
            var query = Read<Organize>(t => t.F_DeleteMark != true);
            query = string.IsNullOrEmpty(keyword) ? query : query.Where(t => t.F_Id.Contains(keyword));
            return query.OrderBy(t => t.F_SortCode).ToListAsync().Result;
        }
        /// <summary>
        /// 取出所有子机构
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="ifRecursion"></param>
        /// <returns></returns>
        public List<Organize> GetListByParentId(string parentId = null)
        {
            var list = new List<Organize>();
            var query = Read<Organize>();
            if (!string.IsNullOrEmpty(parentId))
            {
                list = query.Where(t => t.F_ParentId == parentId && t.F_EnabledMark != true).OrderBy(t => t.F_SortCode).ToListAsync().Result;
                if (true)
                {
                    foreach (var org in list)
                    {
                        list = list.Concat(GetListByParentId(org.F_Id)).ToList();
                    }
                }
            }
            return list;
        }
        public string GetClassInfosByDivisId(string divisId, ref List<Organize> classList)
        {
            var sysOrganizeList = Read<Organize>();
            var divis = sysOrganizeList.FirstOrDefault(p => p.F_Id.Equals(divisId));
            if (divis == null) throw new Exception("未找到该学部");
            var gradeList = sysOrganizeList.Where(p => p.F_ParentId.Equals(divisId)).Select(p => p.F_Id).ToList();
            classList = sysOrganizeList.Where(p => gradeList.Contains(p.F_ParentId)).ToList();
            return divis.F_FullName;
        }
        public void GetClassInfosByGradeId(string gradeId, ref List<Organize> classList, ref List<Organize> divisList)
        {
            var sysOrganizeList = Read<Organize>();
            divisList = sysOrganizeList.Where(p => p.F_ParentId.Equals(gradeId)).ToList();
            var divisIds = divisList.Select(p => p.F_Id).ToList();
            classList = sysOrganizeList.Where(p => divisIds.Contains(p.F_ParentId)).ToList();
        }
        public Organize GetById(string id) => Get<Organize>(id);

        public void CreateOrUpdate(Organize dto, string id)
        {
            
        }

        public List<Organize> GetListByOrgId(string orgIds)
        {
            var query = Read<Organize>();
            if (string.IsNullOrEmpty(orgIds))
            {
                var arrIds = orgIds.Split(',');
                query = query.Where(p => !arrIds.Contains(p.F_Id));
            }
            return query.ToListAsync().Result;
        }

        // 根据类别获取数据
        public List<Organize> GetListByCategory(string categoryId, string parentId)
        {
            var query = Read<Organize>(t => t.F_CategoryId == categoryId && t.F_DeleteMark != true);
            query = string.IsNullOrEmpty(parentId) ? query : query.Where(t => t.F_ParentId == parentId);
            return query.OrderBy(t => t.F_CreatorTime).ToListAsync().Result;
        }

        public void Import(List<Organize> datas)
        {
           
        }


        public void Add(Organize org) => AddAndSave(org);

        public void Delete(string id)
        {
            if (Read<Organize>(t => t.F_ParentId.Equals(id) && t.F_DeleteMark != true).Any()) throw new Exception("删除失败！操作的对象包含了下级数据。");
            var org = Get<Organize>(id);
            if (org == null) return;
            org.F_DeleteMark = true;
            org.Modify(id);
            SaveChanges();
        }


    }
}