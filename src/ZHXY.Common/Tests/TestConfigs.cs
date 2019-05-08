using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestConfigs
    {
        [TestMethod]
        [DataRow("test")]
        public void TestGetValue(string key)
        {
            var val=Configs.GetValue(key);
            System.Console.WriteLine(val);
        }

        [TestMethod]
        [DataRow("name","yujf")]
        [DataRow("age","18")]
        public void TestSetValue(string key,string value)
        {
            Configs.SetValue(key,value);
            var v = Configs.GetValue(key);
            Assert.IsNotNull(v);
            Assert.AreEqual(v,value);

        }
    }
}