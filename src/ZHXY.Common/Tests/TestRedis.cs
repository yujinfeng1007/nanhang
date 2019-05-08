using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestRedis
    {

        [TestMethod]
        [DataRow("name", "yuyuyu")]
        [DataRow("axc", "xsdfasd")]
        [DataRow("sadx", "sadasd")]
        [DataRow("xsxsa", "asxcascs")]
        public void TestString(string key, string value)
        {
            var db = RedisHelper.GetDatabase(1);
            db.StringSet(key, value);
            var getValue = db.StringGet(key);
            Assert.AreEqual(getValue, value);
        }
    }


    [TestClass]
    public class TestDateHelper
    {

        [TestMethod]
        public void TestGetStartTimeAndEndTime()
        {
            //var date = DateHelper.GetStartTimeOfWeek();
            //System.Console.WriteLine(date);
            //date = DateHelper.GetEndTimeOfWeek();
            //System.Console.WriteLine(date);
            //date = DateHelper.GetStartTimeOfMonth();
            //System.Console.WriteLine(date);
            //date = DateHelper.GetEndTimeOfMonth();
            //System.Console.WriteLine(date);
        }
    }
}