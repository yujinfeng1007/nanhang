using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestCaptcha
    {
        [TestMethod]
        [DataRow(1000)]
        public void TestGen(int times)
        {
            for (var i = 0; i < times; i++)
            {
                var (code, img) = CaptchaGenerator.Gen();
                Assert.IsNotNull(code);
                Assert.IsNotNull(img);
                Assert.IsTrue(code.Length == 4);
            }
        }
    }
}