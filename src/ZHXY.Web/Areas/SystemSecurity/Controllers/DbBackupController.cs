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
            dbBackupEntity.FilePath = Server.MapPath("~/Resource/DbBackup/" + dbBackupEntity.FileName + ".bak");
            dbBackupEntity.FileName = dbBackupEntity.FileName + ".bak";
            App.Add(dbBackupEntity);
            return Message("操作成功。");
        }

        [HttpPost]
        
        [HandlerAuthorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteForm(string keyValue)
        {
            App.Delete(keyValue);
            return Message("删除成功。");
        }

        [HttpPost]
        [HandlerAuthorize]
        public void DownloadBackup(string keyValue)
        {
            var data = App.GetById(keyValue);
            var filename = Server.UrlDecode(data.FileName);
            var filepath = Server.MapPath(data.FilePath);
            if (FileDownHelper.FileExists(filepath))
            {
                FileDownHelper.DownLoadold(filepath, filename);
            }
        }
    }
}