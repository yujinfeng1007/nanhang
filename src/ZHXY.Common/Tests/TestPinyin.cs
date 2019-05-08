using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestPinyin
    {
        [TestMethod]
        [DataRow("你好", "NH")]
        [DataRow("abc", "abc")]
        [DataRow("abc 你好呀", "abc NHY")]
        [DataRow("abc你好呀", "abcNHY")]
        public void TestGetFirstPinyin(string text, string pinyin)
        {
            var r = text.GetFirstPinyin();
            Assert.AreEqual(r, pinyin);
        }


        [TestMethod]
        [DataRow("你好", "NiHao")]
        [DataRow("abc", "abc")]
        [DataRow("abc你好呀", "abcNiHaoYa")]
        [DataRow("a b c你好呀", "a b cNiHaoYa")]
        public void TestGetPinyin(string text, string pinyin)
        {
            var r = text.GetFullPinyin();
            Assert.AreEqual(r, pinyin);
        }
    }
}