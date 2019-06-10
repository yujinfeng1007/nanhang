using System;
using System.IO;
using System.Net;

namespace ZHXY.Application
{
    public class GetImageBase64Str
    {
        public static string ImageBase64Str(string ImageURI)
        {
            var filepath = System.AppDomain.CurrentDomain.BaseDirectory;
            var fileName =filepath+ "/image.png";
            var flag = DownLoadPic(ImageURI, fileName);
            if (flag)
            {
                var stream = FileToStream(fileName);
                var by = StreamToBytes(stream);
                return Convert.ToBase64String(by);
            }
            else
            {
                return null;
            }
        }

        public static Stream FileToStream(string fileName)
        {
            // 打开文件
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            var bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            return stream;


        }
        public static byte[] StreamToBytes(Stream stream)
        {
            var bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            return bytes;
        }

        public static bool DownLoadPic(string url, string fileName)
        {
            var flag = true;
            try
            {
                var request = WebRequest.CreateHttp(url);
                request.Method = "GET";
                var response = request.GetResponse() as HttpWebResponse;
                var stream = response.GetResponseStream();
                Stream fileStream = new FileStream(fileName, FileMode.Create);
                var bArr = new byte[1024];
                int size;
                do
                {
                    size = stream.Read(bArr, 0, (int)bArr.Length);
                    fileStream.Write(bArr, 0, size);
                } while (size > 0);
                fileStream.Close();
                stream.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }
    }
}
