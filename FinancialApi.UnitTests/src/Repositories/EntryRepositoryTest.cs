using System;
using FinancialApi.Config;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
    [TestFixture]
    public class EntryRepositoryTest
    {
        private EntryRepository _repository = null;

        [SetUp]
        public void Setup()
        {
            var context = DbHelper.Connection();
            _repository = new EntryRepository(context);
        }

        [Test]
        public void TestSaveCorrectPaymentRepository()
        {
            var entry = PaymentFactory.Build();

            _repository.Save(entry);
            Assert.IsTrue(true, "Save data");
        }


        //[Test]
        //public void TestSaveConcurrency()
        //{
        //    var entry = PaymentFactory.Build();
        //    _repository.Save(entry);


        //    var result1 = _repository.Find(entry.Id.GetValueOrDefault());
        //    var result2 = _repository.Find(entry.Id.GetValueOrDefault());

        //    result1.Value = 100.00m;
        //    _repository.Update(result1);


        //    result2.Value = 100.01m;
        //    _repository.Update(result2);

        //    Assert.IsNull(result2.RowVersion);
        //}



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