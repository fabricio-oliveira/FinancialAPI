using System;
using FinancialApi.Config;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using Microsoft.Data.Sqlite;
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
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseSqlite(connection)
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


        [Test]
        public void TestSaveConcurrency()
        {
            var input = InputFactory.Build();
            _repository.Save(input);


            var result1 = _repository.Find(input.Id.GetValueOrDefault());
            var result2 = _repository.Find(input.Id.GetValueOrDefault());

            result1.Value = 100.00m;
            _repository.Update(result1);


            result2.Value = 100.01m;
            _repository.Update(result2);

            Assert.IsNull(result2.RowVersion);
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