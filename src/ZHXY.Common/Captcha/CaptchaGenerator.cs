using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace ZHXY.Common
{
    /// <summary>
    /// 验证码生成器
    /// </summary>
    public static class CaptchaGenerator
    {
        #region private

        private static readonly Color[] Colors = {
            Color.Black,
            Color.Red,
            Color.Blue,
            Color.Green,
            Color.Orange,
            Color.Brown,
            Color.DarkBlue,
            Color.LightSeaGreen,
            Color.Aqua,
            Color.BlueViolet,
            Color.CadetBlue,
            Color.Chartreuse,
            Color.CornflowerBlue,
            Color.Cyan,
            Color.DarkGreen,
            Color.DarkRed,
            Color.Crimson,
            Color.DarkOrchid,
            Color.DeepPink,
            Color.Fuchsia
        };

        private static readonly string[] Fonts = { "Microsoft YaHei UI" };
        private const int CODE_WIDTH = 80;
        private const int CODE_HEIGHT = 30;
        private const byte FONT_SIZE = 16;

        private static readonly char[] Characters = {
            '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'd', 'e', 'f', 'h', 'k', 'm', 'n', 'r', 'x', 'y',
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y'
        };

        #endregion private

        /// <summary>
        /// 生成验证码
        /// </summary>
        public static (string code, byte[] img) Gen()
        {
            var key = string.Empty;
            var rnd = RandomHelper.GetRandom();

            //生成验证码字符串
            for (var i = 0; i < 4; i++) key += Characters[rnd.Next(Characters.Length)];

            //创建画布
            var bmp = new Bitmap(CODE_WIDTH, CODE_HEIGHT);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            //画噪线
            for (var i = 0; i < 3; i++)
            {
                var x1 = rnd.Next(CODE_WIDTH);
                var y1 = rnd.Next(CODE_HEIGHT);
                var x2 = rnd.Next(CODE_WIDTH);
                var y2 = rnd.Next(CODE_HEIGHT);
                var clr = Colors[rnd.Next(Colors.Length)];
                g.DrawLine(new Pen(clr), x1, y1, x2, y2);
            }

            //画验证码字符串
            for (var i = 0; i < key.Length; i++)
            {
                var fnt = Fonts[rnd.Next(Fonts.Length)];
                var ft = new Font(fnt, FONT_SIZE);
                var clr = Colors[rnd.Next(Colors.Length)];
                g.DrawString(key[i].ToString(), ft, new SolidBrush(clr), (float)i * 18, 0);
            }

            //将验证码图片写入内存流，并将其以 "image/Png" 格式输出
            var ms = new MemoryStream();
            try
            {
                bmp.Save(ms, ImageFormat.Png);
                return (key, ms.ToArray());
            }
            catch { throw; }
            finally
            {
                g.Dispose();
                bmp.Dispose();
            }
        }
    }
}