using NFine.Application.SchoolManage;
using NFine.Application.SystemManage;
using NFine.Code;

using NFine.Domain.Entity.SchoolManage;
using NFine.Domain.Entity.SystemManage;
using System;
using System.Web.Mvc;

namespace NFine.Web.Areas.SchoolManage.Controllers
{
    public class School_NoticeController : ControllerBase
    {
        // GET: /SchoolManage/Notice/

        private NoticeApp noticeapp = new NoticeApp();

        public ActionResult LimRead()
        {
            return View();
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetNoticeJson(Pagination pagination, string F_IsFront, string keyword, string F_Class, string F_Type)
        {
            var data = new
            {
                rows = noticeapp.GetListLim(pagination, F_IsFront, F_Class, F_Type, keyword),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            //var data = noticeapp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetIndexNoticeJson(Pagination pagination, string keyword, string F_Class, string F_Type)
        {
            var data = new
            {
                rows = noticeapp.GetIndexList(pagination, keyword, F_Class, F_Type),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            //var data = noticeapp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        public ActionResult GetListJson(Pagination pagination, string keyword, string F_IsFront)
        {
            var data = new
            {
                //rows = noticeapp.GetListLim(pagination, F_IsFront, keyword),
                total = pagination.Total,
                page = pagination.Page,
                records = pagination.Records
            };
            //var data = noticeapp.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [HandlerAjaxOnly]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = noticeapp.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [HandlerAjaxOnly]
        //[ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult SubmitForm(Notice school_notice, string keyValue)
        {
            //if (!string.IsNullOrEmpty(school_notice.Files))
            //    school_notice.Banner = school_notice.Files.Substring(school_notice.Files.LastIndexOf("/")+1);
            try
            {
                if (!string.IsNullOrEmpty(school_notice.F_Lim_Orgs))
                {
                    string[] lim_org = school_notice.F_Lim_Orgs.Split(',');
                    for (int i = 0; i < lim_org.Length; i++)
                    {
                        Organize orgentity = new OrganizeApp().GetForm(lim_org[i]);
                        if (orgentity != null)
                        {
                            school_notice.F_Lim_OrgsName += orgentity.F_Id + "," + orgentity.F_FullName + "|";
                        }
                        //school_notice.F_Lim_OrgsName += orgentity.F_Id + "," + orgentity.F_FullName + "|";
                    }
                }
                if (!string.IsNullOrEmpty(school_notice.F_Lim_Roles))
                {
                    string[] lim_role = school_notice.F_Lim_Roles.Split(',');
                    for (int i = 0; i < lim_role.Length; i++)
                    {
                        Role roleentity = new RoleApp().GetForm(lim_role[i]);
                        if (roleentity != null)
                        {
                            school_notice.F_Lim_RolesName += roleentity.F_Id + "," + roleentity.F_FullName + "|";
                        }
                    }
                }

                //string[] lim_orgName=null;
                //string[] lim_roleName=null;
                if (string.IsNullOrEmpty(school_notice.F_Lim_Orgs))
                {
                    school_notice.F_Lim_Orgs = "";
                    school_notice.F_Lim_OrgsName = "";
                }
                if (string.IsNullOrEmpty(school_notice.F_Lim_Roles))
                {
                    school_notice.F_Lim_Roles = "";
                    school_notice.F_Lim_RolesName = "";
                }

                school_notice.F_School = OperatorProvider.Provider.GetCurrent().CompanyId;
                school_notice.F_auditdtm = DateTime.Now;
                school_notice.F_auditor = OperatorProvider.Provider.GetCurrent().UserName;
                if (noticeapp.SubmitForm(school_notice, keyValue) > 0)
                {
                    return Success("操作成功。");
                }
                else { return Error("操作失败!"); }
            }
            catch
            {
                return Error("操作失败!");
            }
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Publish(Notice school_notice, string keyValue, int? F_Status)
        {
            string[] F_Id = keyValue.Split('|');
            for (int i = 0; i < F_Id.Length - 1; i++)
            {
                noticeapp.Publish(school_notice, F_Id[i], F_Status);
            }
            return Success("操作成功。");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult UpdeteLimRead(Notice entity, string keyValue)
        {
            entity.F_Id = keyValue;
            noticeapp.UpdateForm(entity);
            return Success("操作成功！");
        }

        [HttpPost]
        [HandlerAjaxOnly]
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            try
            {
                noticeapp.DeleteForm(keyValue);
                return Success("删除成功！");
            }
            catch { return Success("删除失败！。"); }
        }

        //导出excel
        //[HttpGet]
        //public FileResult export(string keyword)
        //{
        ///////////////////获得数据集合
        //Pagination pagination = new Pagination();
        ////排序
        //pagination.sord = "desc";
        ////排序字段
        //pagination.sidx = "F_CreatorTime desc";
        //pagination.rows = 1000000;
        //pagination.page = 1;
        //List<School_NoticeEntity> notice = noticeapp.GetList(pagination, keyword, string.Empty, string.Empty);

        ////////////////////定义规则：字段名，表头名称，字典
        ////字段名->string[]{表头,字典}，若是一般字段 字典为空字符串
        //IDictionary<string, string[]> rules = new Dictionary<string, string[]>();
        //rules.Add("F_Id", new string[] { "编号", string.Empty });
        //rules.Add("Title", new string[] { "标题", string.Empty });
        //rules.Add("Banner", new string[] { "封面", string.Empty });
        //rules.Add("Detail", new string[] { "内容", string.Empty });
        //rules.Add("Files", new string[] { "附件", string.Empty });
        //rules.Add("Status", new string[] { "发布状态", string.Empty });
        //rules.Add("F_CreatorTime", new string[] { "创建时间", string.Empty });

        ////////////////////处理数据(机构 岗位 等字典替换，过滤不要的字段，修改表头)
        //System.Data.DataTable dt = ListToDataTable(notice, rules);

        /////////////////////写流
        //MemoryStream ms = new NPOIExcel().ToExcelStream(dt, "网站公告");
        //ms.Seek(0, SeekOrigin.Begin);
        //string filename = "网站公告" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
        //return File(ms, "application/ms-excel", filename);
        //}
    }
}