using System;
using System.Diagnostics;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class InterestRepositoryTest
    {
        InterestRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var context = DatabaseHelper.Connection();
            _repository = new InterestRepository(context);
        }

        [TearDown]
        public void Cleanup()
        {
            DatabaseHelper.CleanData();
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(12)]
        public void TestCount(int count)
        {
            var account = AccountFactory.Create();
            for (int i = 0; i < count; i++)
                InterestFactory.Create(x => x.Account = account);

            Assert.AreEqual(count, _repository.Count());
        }

        [Test]
        public void Update()
        {
            var created = InterestFactory.Create();
            created.Value = 300.00m;
            _repository.Update(created);

            var finded = _repository.Find(created.Id);

            Assert.AreEqual(300.00m, finded.Value);
        }

        [Test]
        public void TestSave()
        {
            var interest = InterestFactory.Build();

            _repository.Save(interest);
            Assert.IsNotNull(interest.Id);
        }

        [Test]
        public void TestUpdatedEntityd()
        {
            var created = InterestFactory.Create();

            //update value
            created.Value += 2;
            _repository.Update(created);

            //check
            var finded = _repository.Find(created.Id);
            Assert.AreEqual(created.Value, finded.Value);
        }

        [Test]
        public void TestFindEntityNotFound()
        {
            var interest = _repository.Find(1);
            Assert.IsNull(interest);
        }

        [Test]
        public void TestFindExistentEntity()
        {
            var created = InterestFactory.Create();
            var finded = _repository.Find(created.Id);
            Assert.AreEqual(created.Id, finded.Id);
        }

    }
}