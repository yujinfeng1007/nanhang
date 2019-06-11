using System.Linq;
using System.Linq.Dynamic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;
using ZHXY.Web.Shared;

namespace ZHXY.Web.Controllers
{
    /// <summary>
    /// 获取数据
    /// </summary>
    public class SelectController : BaseController
    {
        private UserService App { get; set; }
        private TeacherService TeacherApp { get; set; }
        private StudentService StudentApp { get; set; }

        public SelectController(UserService app, TeacherService teacherApp, StudentService studentApp)
        {
            App = app;
            TeacherApp = teacherApp;
            StudentApp = studentApp;
        }

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
                query = query.Where(p => p.Name.Contains(keyword));
            }
            pagination.Records = query.Count();
            query = string.IsNullOrEmpty(pagination.Sidx) ? query.OrderByDescending(p => p.Id) : query.Paging(pagination);
            query = query.Skip(pagination.Skip).Take(pagination.Rows);
            var list = (from user in query
                        join org in App.Read<Org>() on user.OrganId equals org.Id into orgJoin
                        from org in orgJoin.DefaultIfEmpty()
                        join role in App.Read<Role>() on user.DutyId equals role.Id into roleJoin
                        from role in roleJoin.DefaultIfEmpty()
                        select new
                        {
                            Id = user.Id,
                            RealName = user.Name,
                            OrgId = org.Id ?? "",
                            OrgName = org.Name ?? "",
                            Duty = role.Name
                        }

                        ).ToList();
            return Result.PagingRst(list, pagination.Records, pagination.Total);
        }

        // add by ben
        [HttpGet]
        public ActionResult GetUserListByUserType(Pagination pagination, string keyword, int? UserType)
        {
            var obj = new object();
            if (UserType == 1)
                obj = StudentApp.GetList(pagination, keyword);
            else
                obj = TeacherApp.GetList(pagination, keyword);

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
            var query = App.Read<Org>();
            if (hasKeyword)
            {
                query = query.Where(p => p.Name.Contains(keyword));
            }
            else
            {
                query = string.IsNullOrEmpty(nodeid)
                 ? query.Where(p => p.ParentId.Equals("1"))
                 : query.Where(p => p.ParentId.Equals(nodeid));
            }
            var list = query.Select(p =>
                new 
                {
                    Id = p.Id,
                    ParentId = p.ParentId,
                    //Level = p.F_Level,
                    Loaded = hasKeyword ? true : false,
                    IsLeaf = p.Children.Count == 0,
                    Expanded = hasKeyword ? true : false,
                    Name = p.Name,
                    ParentName = p.Parent.Name,
                    IsDisabled = false,
                    SortCode = p.Sort.HasValue ? p.Sort.Value : 0
                }).ToList();
            list = list.OrderBy(p => string.IsNullOrEmpty(p.ParentName) ? p.Name : p.ParentName + p.SortCode).ToList();

            return Result.Success(list);
        }

        #endregion HttpGet
    }
}