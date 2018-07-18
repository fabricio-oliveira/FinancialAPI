using System;
using System.Diagnostics;
using FinancialApi.Repositories;
using FinancialApiUnitTests.Factory;
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
           var context = DatabaseHelper.Connection();
           _repository = new EntryRepository(context);
       }

       [Test]
       public void TestSave()
       {
           var entry = EntryFactory.Build();

           _repository.Save(entry);
           Assert.IsNotNull(entry.Id);
       }

       [Test]
       public void TestFindEntityNotFound()
       {
           var entry = _repository.Find(1);
           Assert.IsNull(entry);
       }

       [Test]
       public void TestFindExistentEntity()
       {
           var created = EntryFactory.Create();
           var finded = _repository.Find(created.Id.GetValueOrDefault());
           Assert.AreEqual(created.Id, finded.Id);
       }

       [Test]
       public void TestUpdatedEntityd()
       {
           var created = EntryFactory.Create();

           //update value
           created.Value += 2;
           _repository.Update(created);

           //check
           var finded = _repository.Find(created.Id.GetValueOrDefault());
           Assert.AreEqual(created.Value, finded.Value);
       }
   }
}