using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestRandom
    {
        [TestMethod]
        public void TestGetRandom()
        {
            var r = RandomHelper.GetRandom();
            Assert.IsNotNull(r);
            Assert.IsNotNull(r.Next());
        }

        [TestMethod]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(6)]
        [DataRow(7)]
        public void TestNext(int max)
        {
            var r = RandomHelper.Next(max);
            Assert.IsTrue(r < max && r >= 0);
        }
    }
}