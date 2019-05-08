using System.Text.RegularExpressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestRegularRule
    {
        [TestMethod]
        [DataRow("13392819007")]
        [DataRow("8613392819007")]
        [DataRow("013392819007")]
        [DataRow("1795113392819007")]
        public void TestPhoneNumber(string str)
        {
            var regex = new Regex(RegularRule.PHONE_NUMBER);
            var r = regex.Match(str).Success;
            Assert.IsTrue(r);
        }


        [TestMethod]
        [DataRow("13392819007@qq.com")]
        public void TestEmail(string str)
        {
            var regex = new Regex(RegularRule.EMAIL);
            var r = regex.Match(str).Success;
            Assert.IsTrue(r);
        }

        [TestMethod]
        [DataRow("http://www.sina.com")]
        public void TestURL(string str)
        {
            var regex = new Regex(RegularRule.URL);
            var r = regex.Match(str).Success;
            Assert.IsTrue(r);
        }

        [TestMethod]
        [DataRow("2019-12-12")]
        [DataRow("2019/1/1")]
        [DataRow("2019_12_12")]
        [DataRow("2019.2.12")]
        [DataRow("2019/12.12")]
        [DataRow("2019-12_12")]
        [DataRow("2019-12.12")]
        public void TestDate(string str)
        {
            var regex = new Regex(RegularRule.DATE);
            var r = regex.Match(str).Success;
            Assert.IsTrue(r);
        }

        [TestMethod]
        [DataRow("421127199010070912")]
        [DataRow("42134519901007091x")]
        [DataRow("42134519901007091X")]

        public void TestChineseIdCard(string str)
        {
            var regex = new Regex(RegularRule.CHINESE_ID_CARD);
            var r = regex.Match(str).Success;
            Assert.IsTrue(r);
        }

   
    }
}