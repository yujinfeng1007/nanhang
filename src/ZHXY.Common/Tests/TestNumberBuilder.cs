using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestNumberBuilder
    {
        [TestMethod]
        public void TestBuilderNumber()
        {
            var number = NumberBuilder.Build_18bit();
            Assert.AreEqual(number.Substring(0, 12), DateTime.Now.ToString("yyyyMMddHHmm"));
        }
    }
}