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
        public void TestToUndeScoreFirstScenarie(string val, string expected)
        {
            var result = val.ToUnderScore();
            Assert.AreEqual(expected, result);
        }


    }
}
