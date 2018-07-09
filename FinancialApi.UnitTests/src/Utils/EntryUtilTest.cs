using System;
using System.Collections.Generic;
using FinancialApi.src.Utils;
using FinancialApi.Utils;
using FinancialApiUnitTests.Factory;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FinancialApiTests.Utils
{
    public class EntryTest
    {
        [SetUp]
        public void Setup()
        {
        }

        static Object[] TestJsonAttributes =
        {
            new object[] { "ID", new Model(1, "teste"), "id" },
            new object[] { "SomeString", new Model(1, "teste"), "some_string" }
        };

        [TestCaseSource("TestJsonAttributes")]
        public void TestToJson(string key, Object val, string expected)
        {
            var result = val.GetJSonFieldName(key);
            Assert.AreEqual(expected, result);
        }

        public class Model
        {
            public Model(long id, string ss)
            {
                this.ID = id;
                this.SomeString = ss;
            }

            [JsonProperty(PropertyName = "id")]
            public long ID { get; set; }
            [JsonProperty(PropertyName = "some_string")]
            public string SomeString { get; set; }
        }
    }
}
