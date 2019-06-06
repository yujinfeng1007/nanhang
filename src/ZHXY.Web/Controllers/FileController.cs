using System;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Mvc;
using ZHXY.Application;
using ZHXY.Common;

namespace ZHXY.Web.Controllers
{
    public class FileController : Controller
    {
        #region private
        /// <summary>
        /// 允许的扩展名
        /// </summary>
        private static string AllowedExtension { get; } = ConfigurationManager.AppSettings["existen"];

        /// <summary>
        /// 验证是否允许
        /// </summary>
        private static bool IsAllowed(string fileFlag) => AllowedExtension.Contains(fileFlag);

        #endregion

        public ActionResult FileUpload(string moban)
        {
            var message = string.Empty;
            var existen = string.Empty;
            var filepath = string.Empty;
            double size = 0;
            object state = null;
            try
            {
                var privateCloud = ConfigurationManager.AppSettings["PrivateCloud"];
                var mapFormat = ConfigurationManager.AppSettings["MapFormat"];
                var mapPath = ConfigurationManager.AppSettings["MapPath"] + DateTime.Now.ToString("yyyyMMdd") + "/";
                if (privateCloud == "Yes")
                {
                   
                    var UrlPath = System.Web.HttpContext.Current.Request.UrlReferrer.LocalPath.Split('/');
                    var localPath = UrlPath[UrlPath.Length - 2];
                    mapPath = ConfigurationManager.AppSettings["MapPath"] +  "/" + localPath + "/" + DateTime.Now.ToString("yyyyMMdd") + "/";
                }
                var basePath = Server.MapPath(mapPath);
                var files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    if (!Directory.Exists(basePath)) Directory.CreateDirectory(basePath);
                    var random = RandomHelper.GetRandom();
                    var todayStr = DateTime.Now.ToString("yyyyMMddHHmmss");
                    for (var i = 0; i < files.Count; i++)
                    {
                        var strRandom = random.Next(1000, 10000).ToString(); //生成编号
                        var uploadName = $"{todayStr}{strRandom}";
                        existen = files[i].FileName.Substring(files[i].FileName.LastIndexOf('.') + 1);
                        if (!IsAllowed(existen))//files[i]
                        {
                            return Content(new { state = ResultState.Error, message = "上传文件格式有误！" }.Serialize());
                        }
                        var fullPath = $"{basePath}{uploadName}.{existen}";
                        files[i].SaveAs(fullPath);
                        size = Convert.ToDouble(files[i].ContentLength) / 1024;
                        if (string.IsNullOrEmpty(moban))
                        {
                            var s = mapPath.Substring(1, mapPath.Length - 1);
                            FtpHelper.StartUpload(fullPath, s);
                        }
                        message = "上传成功！";
                        state = ResultState.Success;
                        filepath = $"{mapPath}{uploadName}.{existen}";
                    }
                }
                else
                {
                    message = "上传的文件不能为空！";
                    state = ResultState.Error;
                }
            }
            catch
            {
                return Content(new { state = "error", message = "上传错误！" }.Serialize());
            }
            return Json(new { state, message, url = filepath, filpath = filepath, uploadname = filepath, type = existen, size = size.ToString("F") + "KB" });
        }

        [HttpGet]
        public ActionResult DownFile(string filePath, string fileName, string moban)
        {
            var localDir = Server.MapPath(filePath);
            if (string.IsNullOrEmpty(moban))
            {
                var loca = Server.MapPath(filePath.Substring(0, 16));
                FtpHelper.DownloadFile(filePath.Substring(1), loca);
            }
            var fs = new FileStream(localDir, FileMode.Open);
            var bytes = new byte[(int)fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Server.UrlEncode(fileName));
            Response.BinaryWrite(bytes);
            Response.Flush();
            Response.End();
            if (string.IsNullOrEmpty(moban))
            {
                System.IO.File.Delete(localDir);
            }
            return new EmptyResult();
        }
    }
}