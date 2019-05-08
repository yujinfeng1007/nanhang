namespace ZHXY.Common
{
    public static class FileSizeHelper
    {
        /// <summary>
        /// 计算文件大小函数(保留两位小数),Size为字节大小
        /// </summary>
        public static string ToFileSizeString(this long size)
        {
            if (size < 1024.00)
                return size.ToString("F2") + " 字节";
            if (size >= 1024.00 && size < 1048576)
                return (size / 1024.00).ToString("F2") + " KB";
            if (size >= 1048576 && size < 1073741824)
                return (size / 1024.00 / 1024.00).ToString("F2") + " MB";
            if (size >= 1073741824)
                return (size / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return string.Empty;
        }
    }
}