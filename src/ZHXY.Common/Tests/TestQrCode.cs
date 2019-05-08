using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestQrCode
    {
        [TestMethod]
        [DataRow("你好123", 100, 100)]
        [DataRow("dasdsa", 100, 100)]
        [DataRow("123456", 100, 100)]
        [DataRow("http://www.sina.com", 100, 100)]
        public void TestGenerateAndDecode(string strMessahe, int width, int height)
        {
            var r = QRCodeHelper.GenerateQRCode(strMessahe, width, height);
            Assert.AreEqual(r.Height, height);
            Assert.AreEqual(r.Width, width);
            var str = QRCodeHelper.DecodeQRCode(r);
            Assert.AreEqual(str, strMessahe);
        }

    }
}