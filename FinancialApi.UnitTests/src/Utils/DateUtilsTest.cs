using System;
using FinancialApi.Utils;
using NUnit.Framework;

namespace FinancialApiTests.Utils
{
    public class DateUtilTest
    {
        [SetUp]
        public void Setup()
        {
        }

        static object[] _testData = 
        {
            new object[] { new DateTime(2017,01,01),"01-01-2017"}
        };

        [Test]
        public void TestMethod([ValueSource("_testData")]object[] testData)
        {
            var result = ((DateTime) testData[0]).DayFormat();

            Assert.AreEqual(result,testData[1]);

        }
    }
}
