using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;

namespace ZHXY.Common
{
    [TestClass]
    public class TestJson
    {
        private class Animal
        {
            public string Name { get; set; }
            public byte Age { get; set; }
        }

        [TestMethod]
        [DataRow("jdas", 12)]
        [DataRow("asd", 134)]
        [DataRow("dasdsa", 23)]
        [DataRow("asdas", 34)]
        [DataRow("dasdas", 54)]
        public void TestJsonConvert(string name, int age)
        {
            var a = new Animal { Name = name, Age = (byte)age };
            var json = a.ToJson();
            System.Console.WriteLine(json);
            var b = json.Deserialize<Animal>();
            Assert.AreEqual(a.Name, b.Name);
            Assert.AreEqual(a.Age, b.Age);

        }

    }
}