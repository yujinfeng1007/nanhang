using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class SelectController : ZhxyWebControllerBase
    {
        private SysUserAppService App { get; set; }

        public SelectController(SysUserAppService app) => App = app;

        #region View

        [HttpGet]
        public async Task<ActionResult> SelectUser() => await Task.Run(() => View());

        // add by ben
        [HttpGet]
        public async Task<ActionResult> SelectUserByType() => await Task.Run(() => View());

        // add by ben
        [HttpGet]
        public async Task<ActionResult> SelectClassRoom() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectSupplier() => await Task.Run(() => View());

        /*
        [HttpGet]
        public async Task<ActionResult> SelectOrg()
        {
            return await Task.Run(() => View());
        }*/

        [HttpGet]
        public async Task<ActionResult> SelectOwner() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectWarehouse() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectDept() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectGoods() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectGoodsCategory() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectAssetCategory() => await Task.Run(() => View());

        [HttpGet]
        public async Task<ActionResult> SelectCanOutGoods() => await Task.Run(() => View());

        [HttpGet]
        public ActionResult SelectStudent() => View();

        [HttpGet]
        public ActionResult TeacherSelect() => View();

        public ActionResult SelectOrgUser()
        {
            return View();
        }
        #endregion View

        #region HttpGet

        [HttpGet]
        public ActionResult GetUserList(Pagination pagination, string keyword)
        {
            var hasKeyword = !string.IsNullOrEmpty(keyword);
            var query = App.Read<User>();
            if (hasKeyword)
            {
                query = query.Where(p => p.F_RealName.Contains(keyword));
            }
            pagination.Records = query.Count();
            query = string.IsNullOrEmpty(pagination.Sidx) ? query.OrderByDescending(p => p.F_CreatorTime) : query.OrderBy($"{pagination.Sidx} {pagination.Sord}");
            query = query.Skip(pagination.Skip).Take(pagination.Rows);
            var list = (from user in query
                        join org in App.Read<Organize>() on user.F_OrganizeId equals org.F_Id into orgJoin
                        from org in orgJoin.DefaultIfEmpty()
                        join role in App.Read<Role>() on user.F_DutyId equals role.F_Id into roleJoin
                        from role in roleJoin.DefaultIfEmpty()
                        select new
                        {
                            Id = user.F_Id,
                            RealName = user.F_RealName,
                            OrgId = org.F_Id ?? "",
                            OrgName = org.F_FullName ?? "",
                            Duty = role.F_FullName
                        }

                        ).ToList();
            return PagingResult(list, pagination.Records, pagination.Total);
        }

        // add by ben
        [HttpGet]
        public ActionResult GetUserListByUserType(Pagination pagination, string keyword, int? UserType)
        {
            var obj = new object();
            if (UserType == 1)
                obj = new StudentAppService().GetList(pagination, keyword, null, null, null, null)
                    .Select(t => new
                    {
                        F_Name = t.F_Name,
                        F_Num = t.F_StudentNum,
                        F_Divis_ID = t.F_Divis_ID
                    });
            else
                obj = new TeacherAppService().GetList(pagination, keyword, null, null, null)
                    .Select(t => new
                    {
                        F_Name = t.F_Name,
                        F_Num = t.F_Num,
                        F_Divis_ID = t.F_Divis_ID
                    });

            var data = new
            {
                rows = obj,
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            return Content(data.ToJson());
        }

    
        [HttpGet]
        public ActionResult GetDeptTree(string nodeid, string keyword)
        {
            var hasKeyword = !string.IsNullOrEmpty(keyword);
            var query = App.Read<Organize>();
            if (hasKeyword)
            {
                query = query.Where(p => p.F_FullName.Contains(keyword));
            }
            else
            {
                query = string.IsNullOrEmpty(nodeid)
                 ? query.Where(p => p.F_ParentId.Equals("1"))
                 : query.Where(p => p.F_ParentId.Equals(nodeid));
            }
            var list = query.Select(p =>
                new TreeView
                {
                    Id = p.F_Id,
                    Parent = p.F_ParentId,
                    //Level = p.F_Level,
                    Loaded = hasKeyword ? true : false,
                    IsLeaf = p.Children.Count == 0,
                    Expanded = hasKeyword ? true : false,
                    Name = p.F_FullName,
                    ParentName = p.Parent.F_FullName,
                    IsDisabled = p.F_EnabledMark.HasValue ? p.F_EnabledMark.Value : false,
                    SortCode = p.F_SortCode.HasValue ? p.F_SortCode.Value : 0
                }).ToList();
            list = list.OrderBy(p => string.IsNullOrEmpty(p.ParentName) ? p.Name : p.ParentName + p.SortCode).ToList();

            return Result(list);
        }

        #endregion HttpGet
    }
}