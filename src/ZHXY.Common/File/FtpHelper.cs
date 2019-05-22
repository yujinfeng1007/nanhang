using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ZHXY.Common
{
    public class FtpHelper
    {
        #region config
        private static string FtpPassword { get; } = ConfigurationManager.AppSettings["ftppassword"];
        private static string FtpUserName { get; } = ConfigurationManager.AppSettings["ftpname"];
        public static string FtpUri { get; set; } = ConfigurationManager.AppSettings["ftpURI"];
        #endregion

        public static void StartUpload(string targetPath, string relativePath)
        {
            Task.Run(() =>
            {
                var filerelativePath = relativePath.TrimEnd('/').Replace("/", "\\");
                UploadFile(targetPath, filerelativePath);
            });
        }

        private static bool MethodInvoke(string method, Action action)
        {
            if (action != null)
            {
                action();
                return true;
            }

            return false;
        }

        private static T MethodInvoke<T>(string method, Func<T> func)
        {
            if (func != null) return func();

            return default;
        }

        private static FtpWebRequest GetRequest(string uri)
        {
            var result = (FtpWebRequest)WebRequest.Create(uri);
            result.Credentials = new NetworkCredential(FtpUserName, FtpPassword);
            result.KeepAlive = false;
            result.UsePassive = false;
            result.UseBinary = true;
            return result;
        }
        public static bool UploadFile(string filePath, string dirName = "")
        {
            var fileInfo = new FileInfo(filePath);
            if (dirName != "") MakeDir(dirName); //检查文件目录，不存在就自动创建
            var uri = Path.Combine(FtpUri, dirName, fileInfo.Name);
            //Console.WriteLine("start--" + x + "--" + "1");
            return MethodInvoke($@"uploadFile({filePath},{dirName})", () =>
            {
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.UploadFile;
                ftp.ContentLength = fileInfo.Length;
                var buffLength = 2048;
                var buff = new byte[buffLength];
                int contentLen;
                //Console.WriteLine("start--" + x + "--" + "2");
                using (var fs = fileInfo.OpenRead())
                {
                    //Console.WriteLine("start--" + x + "--" + "3");
                    using (var strm = ftp.GetRequestStream())
                    {
                        //Console.WriteLine("start--" + x + "--" + "4");
                        contentLen = fs.Read(buff, 0, buffLength);
                        while (contentLen != 0)
                        {
                            strm.Write(buff, 0, contentLen);
                            contentLen = fs.Read(buff, 0, buffLength);
                        }

                        strm.Close();
                        //Console.WriteLine("start--" + x + "--" + "5");
                    }

                    //fs.Close();
                }

                File.Delete(filePath);
            });
        }
        public static void UploadAllFile(string localDir, string DirName = "")
        {
            string localDirName;
            var targIndex = localDir.LastIndexOf("\\", StringComparison.Ordinal);
            if (targIndex > -1 && targIndex != localDir.IndexOf(":\\", StringComparison.Ordinal) + 1)
                localDirName = localDir.Substring(0, targIndex);
            localDirName = localDir.Substring(targIndex + 1);
            var newDir = Path.Combine(DirName, localDirName);
            MethodInvoke($@"UploadAllFile({localDir},{DirName})", () =>
            {
                MakeDir(newDir);
                var directoryInfo = new DirectoryInfo(localDir);
                var files = directoryInfo.GetFiles();
                //复制所有文件
                foreach (var file in files) UploadFile(file.FullName, newDir);

                //最后复制目录
                var directoryInfoArray = directoryInfo.GetDirectories();
                foreach (var dir in directoryInfoArray) UploadAllFile(Path.Combine(localDir, dir.Name), newDir);
            });
        }

        public static bool DelFile(string filePath)
        {
            var uri = Path.Combine(FtpUri, filePath);
            return MethodInvoke($@"DelFile({filePath})", () =>
            {
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.DeleteFile;
                var response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }
        private static bool DelDir(string dirName)
        {
            var uri = Path.Combine(FtpUri, dirName);
            return MethodInvoke($@"DelDir({dirName})", () =>
            {
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.RemoveDirectory;
                var response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }
        public static bool DelAll(string dirName)
        {
            var list = GetAllFtpFile(new List<ActFile>(), dirName);
            if (list == null) return DelDir(dirName);
            if (list.Count == 0) return DelDir(dirName); //删除当前目录
            var newlist = list.OrderByDescending(x => x.level);
            return MethodInvoke($@"DelAll({dirName})", () =>
            {
                foreach (var item in newlist)
                    if (item.isDir) //判断是目录调用目录的删除方法
                        DelDir(item.path);
                    else
                        DelFile(item.path);

                DelDir(dirName); //删除当前目录
                return true;
            });
        }
        public static bool DownloadFile(string ftpFilePath, string saveDir)
        {
            var filename = ftpFilePath.Substring(ftpFilePath.LastIndexOf("/", StringComparison.Ordinal) + 1);
            var uri = Path.Combine(FtpUri, ftpFilePath);
            return MethodInvoke($@"DownloadFile({ftpFilePath},{saveDir},{filename})", () =>
            {
                if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.DownloadFile;
                using (var response = (FtpWebResponse)ftp.GetResponse())
                {
                    using (var responseStream = response.GetResponseStream())
                    {
                        using (var fs = new FileStream(Path.Combine(saveDir, filename), FileMode.CreateNew))
                        {
                            var buffer = new byte[2048];
                            var read = 0;
                            do
                            {
                                read = responseStream.Read(buffer, 0, buffer.Length);
                                fs.Write(buffer, 0, read);
                            } while (read != 0);

                            responseStream.Close();
                            fs.Flush();
                            fs.Close();
                        }

                        responseStream.Close();
                    }
                }
            });
        }

        public static void DownloadAllFile(string dirName, string saveDir) => MethodInvoke($@"DownloadAllFile({dirName},{saveDir})", () =>
                                                                     {
                                                                         var files = GetFtpFile(dirName);
                                                                         if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

                                                                         foreach (var f in files)
                                                                             if (f.isDir) //文件夹，递归查询
                                                                                 DownloadAllFile(Path.Combine(dirName, f.name), Path.Combine(saveDir, f.name));
                                                                             else //文件，直接下载
                                                                                 DownloadFile(Path.Combine(dirName, f.name), saveDir);
                                                                     });

        public static List<ActFile> GetFtpFile(string dirName, int ilevel = 0)
        {
            var ftpfileList = new List<ActFile>();
            var uri = Path.Combine(FtpUri, dirName);
            return MethodInvoke($@"GetFtpFile({dirName})", () =>
            {
                var a = new List<List<string>>();
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var stream = ftp.GetResponse().GetResponseStream();
                using (var sr = new StreamReader(stream))
                {
                    var line = sr.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        ftpfileList.Add(new ActFile
                        {
                            isDir = line.IndexOf("<DIR>", StringComparison.Ordinal) > -1,
                            name = line.Substring(39).Trim(),
                            path = Path.Combine(dirName, line.Substring(39).Trim()),
                            level = ilevel
                        });
                        line = sr.ReadLine();
                    }

                    sr.Close();
                }

                return ftpfileList;
            });
        }
        public static List<ActFile> GetAllFtpFile(List<ActFile> result, string dirName, int level = 0)
        {
            List<ActFile> ftpfileList;
            return MethodInvoke($@"GetAllFtpFile({dirName})", () =>
            {
                ftpfileList = GetFtpFile(dirName, level);
                result.AddRange(ftpfileList);
                var newlist = ftpfileList.Where(x => x.isDir).ToList();
                foreach (var item in newlist) GetAllFtpFile(result, item.path, level + 1);

                return result;
            });
        }

        public static bool CheckDir(string dirName, string currentDir = "")
        {
            var uri = Path.Combine(FtpUri, currentDir);
            return MethodInvoke($@"CheckDir({dirName}{currentDir})", () =>
            {
                var ftp = GetRequest(uri);
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                var stream = ftp.GetResponse().GetResponseStream();
                using (var sr = new StreamReader(stream))
                {
                    var line = sr.ReadLine();
                    while (!string.IsNullOrEmpty(line))
                    {
                        if (line.IndexOf("<DIR>", StringComparison.Ordinal) > -1)
                            if (line.Substring(39).Trim() == dirName)
                                return true;

                        line = sr.ReadLine();
                    }

                    sr.Close();
                }

                //stream.Close();
                return false;
            });
        }

        public static bool MakeDir(string dirName)
        {
            var dirs = dirName.Split('\\').ToList(); //针对多级目录分割
            var currentDir = string.Empty;
            return MethodInvoke($@"MakeDir({dirName})", () =>
            {
                foreach (var dir in dirs)
                    if (!string.IsNullOrEmpty(dir) && !CheckDir(dir, currentDir)) //检查目录不存在则创建
                    {
                        currentDir = Path.Combine(currentDir, dir);
                        var uri = Path.Combine(FtpUri, currentDir);
                        var ftp = GetRequest(uri);
                        ftp.Method = WebRequestMethods.Ftp.MakeDirectory;
                        var response = (FtpWebResponse)ftp.GetResponse();
                        response.Close();
                    }
                    else
                    {
                        currentDir = Path.Combine(currentDir, dir);
                    }
            });
        }

        public static bool Rename(string currentFilename, string newFilename, string dirName = "")
        {
            var uri = Path.Combine(FtpUri, dirName, currentFilename);
            return MethodInvoke($@"Rename({currentFilename},{newFilename},{dirName})", () =>
            {
                var ftp = GetRequest(uri);
                ftp.Method = WebRequestMethods.Ftp.Rename;
                ftp.RenameTo = newFilename;
                var response = (FtpWebResponse)ftp.GetResponse();
                response.Close();
            });
        }


        /// <summary>
        /// 上传文件
        /// author: yujinfeng
        /// </summary>
        /// <param name="fs">文件流,该方法执行完,流不会自行关闭,请手动关闭</param>
        /// <param name="filePath">文件路径</param>
        public static void UpLoad(Stream fs, string filePath)
        {
            var req = GetRequest(Path.Combine(FtpUri, filePath));
            req.Method = WebRequestMethods.Ftp.UploadFile;
            fs.Seek(0, SeekOrigin.Begin);
            var buffLength = 2048;              // 缓冲大小
            var buff = new byte[buffLength];    // 缓冲管道
            int contentLen;                     // 读到的字节数
            var strm = req.GetRequestStream();
            contentLen = fs.Read(buff, 0, buffLength);
            while (contentLen != 0)
            {
                strm.Write(buff, 0, contentLen);
                contentLen = fs.Read(buff, 0, buffLength);
            }
            strm.Close();
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        public static Stream DownLoad(string filePath)
        {
            var req = GetRequest(Path.Combine(FtpUri, filePath));
            req.Method = WebRequestMethods.Ftp.DownloadFile;
            var strm = req.GetRequestStream();
            var ms = new MemoryStream();
            strm.CopyTo(ms);
            strm.Close();
            return ms;
        }
    }



    public class ActFile
    {
        public int level { get; set; }
        public bool isDir { get; set; }
        public string name { get; set; }
        public string path { get; set; } = "";
    }
}