using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZHXY.Common
{
    [TestClass]
    public class TestAutoMapper
    {
        private class TestSource
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string NickName { get; set; }
            public int age { get; set; }
        }

        private class TestDestination
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Nick_Name { get; set; }
            public string Age { get; set; }
        }

        [TestMethod]
        [DataRow(1, "Jack", "jj", 17)]
        [DataRow(2, "zhang", "jj", 43)]
        [DataRow(3, "li", "jj", 23)]
        [DataRow(4, "LiLy", "jj", 13)]
        public void TestMapTo(int id, string name, string nickName, int age)
        {
            var source = new TestSource { Id = id, Name = name, NickName = nickName, age = age };
            var destination = source.MapTo<TestDestination>();
            Assert.AreEqual(destination.Id, id);
            Assert.AreEqual(destination.Name, name);
            Assert.AreEqual(destination.Nick_Name, nickName);
            Assert.AreEqual(destination.Age, age.ToString());

            var c = new TestDestination();
            var c1 = c;
            source.MapTo(c);
            Assert.AreEqual(c.Id, id);
            Assert.AreEqual(c.Name, name);
            Assert.AreEqual(c.Nick_Name, nickName);
            Assert.AreEqual(c.Age, age.ToString());
            Assert.AreEqual(c1, c);
        }

    }
}