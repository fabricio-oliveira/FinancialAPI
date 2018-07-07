using System;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;
using FinancialApiUnitTests.src.Factory;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class PaymentRepositoryTest
    {
        private PaymentRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                    .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                    .Options;

            var context = new DataBaseContext(options);
            context.Database.EnsureCreated();
            _repository = new PaymentRepository(context);
        }

        [Test]
        public void TestSaveCorrectPaymentRepository()
        {
            var payment = PaymentFactory.Build();
            _repository.Save(payment);
            Assert.IsTrue(true, "Save data");
        }

        [Test]
        public void TestSaveEmptyDescriptionRepository()
        {

            var payment = PaymentFactory.Build();
            payment.Description = null;
            try {
                _repository.Save(payment);
            } 
            catch(DbUpdateException)
            {   
                Assert.IsTrue(true, "Check fail null description");
            }
        }

        [Test]
        public void TestSaveEmptyDestinationAccountRepository()
        {

            var payment = PaymentFactory.Build();
            payment.DestinationAccount = null;
            try
            {
                _repository.Save(payment);
            }
            catch (DbUpdateException)
            {
                Assert.IsTrue(true, "Check Fail null DestinationAccount");
            }
        }
    }
}