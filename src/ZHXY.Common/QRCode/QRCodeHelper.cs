using System.Drawing;
using ZXing;
using ZXing.QrCode.Internal;

namespace ZHXY.Common
{
    /// <summary>
    /// 二维码助手类
    /// author: 余金锋
    /// phone:  l33928l9OO7
    /// email:  2965l9653@qq.com
    public static class QRCodeHelper
    {
        /// <summary>
        ///     生成二维码图片
        /// </summary>
        /// <param name="strMessage"> 要生成二维码的字符串 </param>
        /// <param name="width">      二维码图片宽度 </param>
        /// <param name="height">     二维码图片高度 </param>
        /// <returns>  </returns>
        public static Bitmap GenerateQRCode(
            string strMessage,
            int width,
            int height)
        {
            Bitmap result = null;
            try
            {
                var barCodeWriter = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
                barCodeWriter.Options.Hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");
                barCodeWriter.Options.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
                barCodeWriter.Options.Height = height;
                barCodeWriter.Options.Width = width;
                barCodeWriter.Options.Margin = 0;
                var bm = barCodeWriter.Encode(strMessage);
                result = barCodeWriter.Write(bm);
            }
            catch
            {
                //异常输出
            }

            return result;
        }

        /// <summary>
        ///     解码二维码
        /// </summary>
        /// <param name="barcodeBitmap"> 待解码的二维码图片 </param>
        /// <returns> 扫码结果 </returns>
        public static string DecodeQRCode(Bitmap barcodeBitmap)
        {
            var reader = new BarcodeReader { Options = { CharacterSet = "UTF-8" } };
            var result = reader.Decode(barcodeBitmap);
            return result?.Text;
        }
    }
}