using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace ZHXY.Common
{
    public class FileDownHelper
    {
        public static string FileNameExtension(string FileName) => Path.GetExtension(MapPathFile(FileName));

        public static string MapPathFile(string FileName) => HttpContext.Current.Server.MapPath(FileName);

        public static bool FileExists(string FileName)
        {
            var destFileName = FileName;
            if (File.Exists(destFileName))
                return true;
            return false;
        }

        public static void DownLoadold(string FileName, string name)
        {
            var destFileName = FileName;
            if (File.Exists(destFileName))
            {
                var fi = new FileInfo(destFileName);
                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.ClearHeaders();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AppendHeader("Content-Disposition",
                    "attachment;filename=" + HttpUtility.UrlEncode(name, Encoding.UTF8));
                HttpContext.Current.Response.AppendHeader("Content-Length", fi.Length.ToString());
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.WriteFile(destFileName);
                HttpContext.Current.Response.Flush();
                HttpContext.Current.Response.End();
            }
        }

        public static void DownLoad(string FileName)
        {
            var filePath = MapPathFile(FileName);
            long chunkSize = 204800; //指定块大小
            var buffer = new byte[chunkSize]; //建立一个200K的缓冲区
            FileStream stream = null;
            try
            {
                //打开文件
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var dataToRead = stream.Length; //已读的字节数

                //添加Http头
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition",
                    "attachement;filename=" + HttpUtility.UrlEncode(Path.GetFileName(filePath)));
                HttpContext.Current.Response.AddHeader("Content-Length", dataToRead.ToString());

                while (dataToRead > 0)
                    if (HttpContext.Current.Response.IsClientConnected)
                    {
                        var length = stream.Read(buffer, 0, Convert.ToInt32(chunkSize));
                        HttpContext.Current.Response.OutputStream.Write(buffer, 0, length);
                        HttpContext.Current.Response.Flush();
                        HttpContext.Current.Response.Clear();
                        dataToRead -= length;
                    }
                    else
                    {
                        dataToRead = -1; //防止client失去连接
                    }
            }
            catch (Exception ex)
            {
                HttpContext.Current.Response.Write("Error:" + ex.Message);
            }
            finally
            {
                stream?.Close();
                HttpContext.Current.Response.Close();
            }
        }

        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName,
            string _fullPath, long _speed)
        {
            try
            {
                var myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                var br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;

                    var fileLength = myFile.Length;
                    long startBytes = 0;
                    var pack = 10240; //10K bytes
                    var sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;

                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        var range = _Request.Headers["Range"].Split('=', '-');
                        startBytes = Convert.ToInt64(range[1]);
                    }

                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                        _Response.AddHeader("Content-Range",
                            $" bytes {startBytes}-{fileLength - 1}/{fileLength}");

                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition",
                        "attachment;filename=" + HttpUtility.UrlEncode(_fileName, Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    var maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (var i = 0; i < maxCount; i++)
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     http下载文件
        /// </summary>
        /// <param name="url">下载文件地址</param>
        /// <param name="path">文件存放地址，包含文件名</param>
        /// <returns></returns>
        public bool HttpDownload(string url, string path)
        {
            var tempPath = Path.GetDirectoryName(path) + @"\temp";
            Directory.CreateDirectory(tempPath); //创建临时文件目录
            var tempFile = tempPath + @"\" + Path.GetFileName(path) + ".temp"; //临时文件
            if (File.Exists(tempFile)) File.Delete(tempFile); //存在则删除
            try
            {
                var fs = new FileStream(tempFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                // 设置参数
                var request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                var response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                var responseStream = response.GetResponseStream();
                //创建本地文件写入流
                //Stream stream = new FileStream(tempFile, FileMode.Create);
                var bArr = new byte[1024];
                var size = responseStream.Read(bArr, 0, bArr.Length);
                while (size > 0)
                {
                    //stream.Write(bArr, 0, size);
                    fs.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, bArr.Length);
                }

                //stream.Close();
                fs.Close();
                responseStream.Close();
                File.Move(tempFile, path);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /**
        * 获取response header中Content-Disposition中的filename值
        * @param response
        * @return
        */

        public static string getFileName(WebResponse response)
        {
            var fileinfo = response.Headers["Content-Disposition"];
            const string mathkey = "filename=";
            return fileinfo.Substring(fileinfo.LastIndexOf(mathkey, StringComparison.Ordinal)).Replace(mathkey, "");
        }
    }
}