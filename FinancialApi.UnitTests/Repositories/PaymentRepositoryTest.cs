using System;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;
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

        private Payment FactoryPayment(){
            return new Payment("Op Teste","1234567-8","0123-4","corrente","012.345.678-90",
                               100.00m, 0.03m, DateTime.Now);
        }

        [Test]
        public void TestSaveCorrectPaymentRepository()
        {
            var payment = FactoryPayment();
            _repository.Save(payment);
            Assert.IsTrue(true, "Save data");
        }

        [Test]
        public void TestSaveEmptyDescriptionRepository()
        {

            var payment = FactoryPayment();
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

            var payment = FactoryPayment();
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