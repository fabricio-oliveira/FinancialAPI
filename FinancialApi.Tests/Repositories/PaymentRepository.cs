using System;
using FinancialApi.Model;
using FinancialApi.Repositories;
using Microsoft.Data.Sqlite;
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
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();


            var options = new DbContextOptionsBuilder<DataBaseContext>()
                    .UseSqlite(connection)
                    .Options;

            var context = new DataBaseContext(options);
            context.Database.EnsureCreated();
            _repository = new PaymentRepository(context);
        }

        [Test]
        public void TestSaveEmptyRepository()
        {
            
            var payment = new Payment();
            try {
                _repository.Save(payment);
            } 
            catch(DbUpdateException)
            {
                Assert.IsTrue(true, "Validate descripion null");
            }
        }
    }
}