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

        ///// <summary>
        ///// 无损压缩图片
        ///// </summary>
        ///// <param name="sFile">原图片地址</param>
        ///// <param name="dFile">压缩后保存图片地址</param>
        ///// <param name="flag">压缩质量（数字越小压缩率越高）1-100</param>
        ///// <param name="size">压缩后图片的最大大小</param>
        ///// <param name="sfsc">是否是第一次调用</param>
        ///// <returns></returns>
        //public static bool CompressImage(string sFile, string dFile, int flag = 50, int size = 50, bool sfsc = true)
        //{
        //    //如果是第一次调用，原始图像的大小小于要压缩的大小，则直接复制文件，并且返回true
        //    FileInfo firstFileInfo = new FileInfo(sFile);
        //    if (sfsc == true && firstFileInfo.Length < size * 1024)
        //    {
        //        firstFileInfo.CopyTo(dFile);
        //        return true;
        //    }
        //    Image iSource = Image.FromFile(sFile);
        //    ImageFormat tFormat = iSource.RawFormat;
        //    int dHeight = iSource.Height / 2;
        //    int dWidth = iSource.Width / 2;
        //    int sW = 0, sH = 0;
        //    //按比例缩放
        //    Size tem_size = new Size(iSource.Width, iSource.Height);
        //    if (tem_size.Width > dHeight || tem_size.Width > dWidth)
        //    {
        //        if ((tem_size.Width * dHeight) > (tem_size.Width * dWidth))
        //        {
        //            sW = dWidth;
        //            sH = (dWidth * tem_size.Height) / tem_size.Width;
        //        }
        //        else
        //        {
        //            sH = dHeight;
        //            sW = (tem_size.Width * dHeight) / tem_size.Height;
        //        }
        //    }
        //    else
        //    {
        //        sW = tem_size.Width;
        //        sH = tem_size.Height;
        //    }

        //    Bitmap ob = new Bitmap(dWidth, dHeight);
        //    Graphics g = Graphics.FromImage(ob);

        //    g.Clear(Color.WhiteSmoke);
        //    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
        //    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        //    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

        //    g.DrawImage(iSource, new Rectangle((dWidth - sW) / 2, (dHeight - sH) / 2, sW, sH), 0, 0, iSource.Width, iSource.Height, GraphicsUnit.Pixel);

        //    g.Dispose();

        //    //以下代码为保存图片时，设置压缩质量
        //    EncoderParameters ep = new EncoderParameters();
        //    long[] qy = new long[1];
        //    qy[0] = flag;//设置压缩的比例1-100
        //    EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, qy);
        //    ep.Param[0] = eParam;

        //    try
        //    {
        //        ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
        //        ImageCodecInfo jpegICIinfo = null;
        //        for (int x = 0; x < arrayICI.Length; x++)
        //        {
        //            if (arrayICI[x].FormatDescription.Equals("JPEG"))
        //            {
        //                jpegICIinfo = arrayICI[x];
        //                break;
        //            }
        //        }
        //        if (jpegICIinfo != null)
        //        {
        //            ob.Save(dFile, jpegICIinfo, ep);//dFile是压缩后的新路径
        //            FileInfo fi = new FileInfo(dFile);
        //            if (fi.Length > 1024 * size)
        //            {
        //                flag = flag - 10;
        //                CompressImage(sFile, dFile, flag, size, false);
        //            }
        //        }
        //        else
        //        {
        //            ob.Save(dFile, tFormat);
        //        }
        //        return true;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //    finally
        //    {
        //        iSource.Dispose();
        //        ob.Dispose();
        //    }
        //}
    }
}