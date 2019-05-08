using System.Web.Mvc;
using ZHXY.Application;using ZHXY.Domain;
using ZHXY.Common;

namespace ZHXY.Web.SystemSecurity.Controllers
{
    public class DbBackupController : ZhxyWebControllerBase
    {
        private SysDbBackupAppService App { get; }
        public DbBackupController(SysDbBackupAppService app) => App = app;

        [HttpGet]
        
        public ActionResult GetGridJson(string queryJson)
        {
            var data = App.GetList(queryJson);
            return Content(data.ToJson());
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public ActionResult SubmitForm(DbBackup dbBackupEntity)
        {
            dbBackupEntity.F_FilePath = Server.MapPath("~/Resource/DbBackup/" + dbBackupEntity.F_FileName + ".bak");
            dbBackupEntity.F_FileName = dbBackupEntity.F_FileName + ".bak";
            App.SubmitForm(dbBackupEntity);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.DeleteForm(keyValue);
            return Message("删除成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            var data = App.GetForm(keyValue);
            var filename = Server.UrlDecode(data.F_FileName);
            var filepath = Server.MapPath(data.F_FilePath);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
    }
}