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
            _repository = new AccountRepository(context);
        }

        [Test]
        public void TestSaveCorrectPaymentRepository()
        {
            var entry = AccountFactory.Build();

            _repository.Save(entry);
            Assert.IsTrue(true, "Save data");
        }
    }
}