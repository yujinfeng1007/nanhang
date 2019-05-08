using System.IO;
using System.IO.Compression;
using System.Linq;

namespace ZHXY.Common
{
    /// <summary>
    /// 压缩帮助类
    /// </summary>
    public class CompressionHelper
    {
        public static byte[] DeflateBytes(byte[] bytes)
        {
            if (!bytes.Any()) return null;
            using (var output = new MemoryStream())
            {
                using (var compresstor = new DeflateStream(output, CompressionMode.Compress))
                {
                    compresstor.Write(bytes, 0, bytes.Length);
                }
                return output.ToArray();
            }
        }

        public static byte[] GZipBytes(byte[] bytes)
        {
            if (!bytes.Any()) return null;
            using (var output = new MemoryStream())
            {
                using (var compresstor = new GZipStream(output, CompressionMode.Compress))
                {
                    compresstor.Write(bytes, 0, bytes.Length);
                }
                return output.ToArray();
            }
        }
    }
}