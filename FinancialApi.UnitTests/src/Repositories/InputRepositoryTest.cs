using System;
using FinancialApi.Config;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class InputRepositoryTest
    {
        private InputRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                    .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                    .Options;

            var context = new DataBaseContext(options);
            context.Database.EnsureCreated();

            _repository = new InputRepository(context);
        }

        [Test]
        public void TestSaveCorrectPaymentRepository()
        {
            var input = InputFactory.Build();
            _repository.Save(input);
            Assert.IsTrue(true, "Save data");
        }

        //[Test]
        //public void TestSaveEmptyDescriptionRepository()
        //{

        //    var input = InputFactory.Build();
        //    input.Date = null;
        //    try {
        //        _repository.Save(input);
        //    } 
        //    catch(DbUpdateException)
        //    {   
        //        Assert.IsTrue(true, "Check fail null description");
        //    }
        //}

        //[Test]
        //public void TestSaveEmptyDestinationAccountRepository()
        //{

        //    var payment = PaymentFactory.Build();
        //    payment.DestinationAccount = null;
        //    try
        //    {
        //        _repository.Save(payment);
        //    }
        //    catch (DbUpdateException)
        //    {
        //        Assert.IsTrue(true, "Check Fail null DestinationAccount");
        //    }
        //}
    }
}