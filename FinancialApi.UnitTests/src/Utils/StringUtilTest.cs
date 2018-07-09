using System;
using FinancialApi.Utils;
using NUnit.Framework;

namespace FinancialApiTests.Utils
{
    public class StringTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("camelCase", "camel_case")]
        [TestCase("camel_case", "camel_case")]
        [TestCase("camelcase", "camelcase")]
        [TestCase("CamelCase","camel_case")]
        public void TestToUndeScore(string val, string expected)
        {
            var result = val.ToUnderScore();
            Assert.AreEqual(expected, result);
        }

        static Object[] TestJsonConvertMatch =
        {
            new object[] { new Test(1,"teste"),@"{""ID"":1,""Name"":""teste""}" },
            new object[] { new Test(1, null),  @"{""ID"":1}"}
        };

        [TestCaseSource("TestJsonConvertMatch")]
        public void TestToJson(Object val, string expected)
        {
            var result = val.ToJson();
            Assert.AreEqual(expected, result);
        }
            
        //spec

        private class Test
        {
            public int ID { get; set; }
            public string Name { get; set; }

            public Test(int id, string name){
                this.ID = id;
                this.Name = name;
            }
        }

    }
}
