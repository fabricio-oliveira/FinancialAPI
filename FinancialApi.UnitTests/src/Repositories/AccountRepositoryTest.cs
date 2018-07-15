using System;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class AccountRepositoryTest
    {
        private AccountRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var context = DbHelper.Connection();
            //DbHelper.Cleaner();
            _repository = new AccountRepository(context);
        }

        [Test]
        public void TestSave()
        {
            var entry = AccountFactory.Build();

            _repository.Save(entry);
            Assert.IsNotNull(entry.Id);
        }

        [Test]
        public void TestFindAccountNotFound()
        {
            var entry = _repository.Find(1);
            Assert.IsNull(entry);
        }

        [Test]
        public void TestFindExistentAccount()
        {
            var created = AccountFactory.Create();
            var finded = _repository.Find(created.Id.GetValueOrDefault());
            Assert.AreEqual(created.Id, finded.Id);
        }
    }
}