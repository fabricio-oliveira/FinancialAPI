using System;
using System.Diagnostics;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
using System.Collections.Generic;
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

        [TearDown]
        public void Cleanup()
        {
            DatabaseHelper.CleanData();
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
       public void TestUpdatedEntity()
       {
           var created = BalanceFactory.Create();

           //update value
           created.Total += 2;
           _repository.Update(created);

           //check
           var finded = _repository.Find(created.Id.GetValueOrDefault());
           Assert.AreEqual(created.Total, finded.Total);
       }

         [TestCase(30, 30)]
         [TestCase(20, 20)]
         [TestCase(40, 30)]
         [TestCase(0, 0)]
       public void TestListListTodayMore30Ahead(int input, int output)
       {
           var account = AccountFactory.Create();
           for(int i =0; i < input; i++)
           {
               BalanceFactory.Create( x=> 
               {
                   x.Date = DateTime.Today.AddDays(i);
                   x.Account = account;
               });
           }
           var balances = _repository.ListTodayMore30Ahead(account);
        
           //check
           Assert.AreEqual(output, balances.Count);
       }

         public void TestFindOrCreatedDontHaveBalanceOfDay()
       {
           
           var account = AccountFactory.Create();
           var created = BalanceFactory.Create( x=> {
                                                        x.Date = DateTime.Today;
                                                        x.Account = account;
                                                    });
        

            var finded = _repository.FindOrCreateBy(account,DateTime.Today);
           //check
           Assert.AreEqual(created.Id, finded.Id);
       }

        public void TestFindOrCreatedDontHaveBalanceOfPreviuslyDay()
       {
           
           var account = AccountFactory.Create();
           var created = BalanceFactory.Create( x=> {
                                                        x.Date = DateTime.Today.AddDays(-1);
                                                        x.Account = account;
                                                    });
        

            var finded = _repository.FindOrCreateBy(account,DateTime.Today);
           //check
           Assert.AreNotEqual(created.Id, finded.Id);
           Assert.AreEqual(created.Total, finded.Total);
            Assert.AreEqual(_repository.Count(), 2);
       }


   }
}