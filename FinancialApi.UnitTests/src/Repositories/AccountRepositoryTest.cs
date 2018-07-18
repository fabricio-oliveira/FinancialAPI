using System;
using FinancialApi.Config;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class AccountRepositoryTest
    {
        IAccountRepository _repository;
        DataBaseContext _context;

        [SetUp]
        public void Setup()
        {
            _context = DatabaseHelper.Connection();
            _repository = new AccountRepository(_context);
        }

        [TearDown]
        public void Cleanup()
        {
            DatabaseHelper.CleanData();
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


        [Test]
        public void TestFindOrCreateNotExistAccount()
        {
           var created = AccountFactory.Build();
           var finded = _repository.FindOrCreate(created.Number, created.Bank, created.Type, created.Identity);
           Assert.IsNotNull(finded.Id);
           Assert.AreEqual(_repository.Count(), 1);

        }


        [Test]
        public void TestFindOrCreateExistAccount()
        {
           var created = AccountFactory.Create();
           var finded = _repository.FindOrCreate(created.Number, created.Bank, created.Type, created.Identity);
           Assert.IsNotNull(finded.Id);
           Assert.AreEqual(_repository.Count(), 1);

        }
    }
}