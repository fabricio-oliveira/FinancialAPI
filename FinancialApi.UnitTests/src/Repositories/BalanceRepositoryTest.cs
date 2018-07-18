using System;
using System.Diagnostics;
using FinancialApi.Models.DTO;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.repositories
{
   [TestFixture]
   public class BalaceRepositoryTest
   {
       BalanceRepository _repository;

       [SetUp]
       public void Setup()
       {
           var context = DatabaseHelper.Connection();
           _repository = new BalanceRepository(context);
       }

       [Test]
       public void TestSave()
       {
           var balance = BalanceFactory.Build(x =>
           {
               x.Inputs.Clear();
               x.Inputs.Add(new ShortEntryDTO(DateTime.Today, 100m));
           });

           _repository.Save(balance);
           Assert.IsNotNull(balance.Id);
           Assert.AreEqual(balance.Inputs.Count, 1);
       }

       [Test]
       public void TestFindEntityNotFound()
       {
           var balance = _repository.Find(1);
           Assert.IsNull(balance);
       }

       [Test]
       public void TestFindExistentEntity()
       {
           var created = BalanceFactory.Create();
           var finded = _repository.Find(created.Id.GetValueOrDefault());
           Assert.AreEqual(created.Id, finded.Id);
       }

       [Test]
       public void TestUpdatedEntityd()
       {
           var created = BalanceFactory.Create();

           //update value
           created.Total += 2;
           _repository.Update(created);

           //check
           var finded = _repository.Find(created.Id.GetValueOrDefault());
           Assert.AreEqual(created.Total, finded.Total);
       }
   }
}