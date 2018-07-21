using System;
using System.Collections.Generic;
using FinancialApi.Models.DTO;
using FinancialApi.Models.Entity;
using FinancialApi.Repositories;
using FinancialApi.Services;
using FinancialApiUnitTests.Factory;
using Moq;
using NUnit.Framework;

namespace FinancialApiUnitTests.src.services
{
    public class BalanceServiceTest
    {
        BalanceService _service;

        Mock<IBalanceRepository> _mockBalanceRepository;
        Mock<IAccountRepository> _mockAccountRepository;
        Mock<IInterestRepository> _mockInterestRepository;

        [SetUp]
        public void Setup()
        {
            _mockBalanceRepository = new Mock<IBalanceRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockInterestRepository = new Mock<IInterestRepository>();

            _service = new BalanceService(_mockBalanceRepository.Object,
                                          _mockInterestRepository.Object,
                                          _mockAccountRepository.Object);
        }

        [Test]
        public void TestCashFlowWhenHasAccount()
        {
            //Input
            var account = AccountFactory.Build();

            //behavior
            _mockAccountRepository.Setup(m => m.FindAs(account.Number, account.Bank, account.Type, account.Identity))
                                  .Returns(account);

            var balances = new List<Balance>();
            _mockBalanceRepository.Setup(m => m.ListTodayMore30Ahead(account.Id))
                                      .Returns(balances);


            //test
            var val = _service.CashFlow(account);


            //assert
            Assert.IsInstanceOf<List<Balance>>(val);
            Assert.AreEqual(balances, val);
            _mockBalanceRepository.Verify(x => x.ListTodayMore30Ahead(account.Id), Times.Once());
        }

        [Test]
        public void TestPoProcessCheck()
        {
            //Input
            var account = AccountFactory.Build();
            var date = DateTime.Now;

            //behavior
            var accounts = new List<Account>();
            _mockAccountRepository.Setup(m => m.List())
                                  .Returns(accounts);

            var balances = new List<Balance>();
            _mockBalanceRepository.Setup(m => m.ToProcess(date, accounts))
                                  .Returns(balances);



            //test
            var val = _service.ToProcess(date);


            //assert
            Assert.IsInstanceOf<List<Balance>>(val);
            Assert.AreEqual(balances, val);
            _mockAccountRepository.Verify(x => x.List(), Times.Once());
            _mockBalanceRepository.Verify(x => x.ToProcess(date, accounts), Times.Once());
        }



        [Test]
        public void TestGenerateBalanceWithInterestWithPositiveTotal()
        {
            //Input
            var balance = BalanceFactory.Create(x =>
            {
                x.Total = 30.00m;
                x.Charges = new List<ShortEntryDTO>();
            });

            var date = DateTime.Now;


            //behavior
            _mockBalanceRepository.Setup(m => m.BeginTransaction())
                                  .Verifiable();
            _mockBalanceRepository.Setup(m => m.Commit())
                                  .Verifiable();
            _mockBalanceRepository.Setup(m => m.Update(balance))
                                  .Verifiable();


            //test
            _service.GenerateBalanceWithInterest(balance, date);


            //assert
            Assert.AreEqual(balance.Closed, true);
            Assert.AreEqual(balance.Total, 30.00m);

            _mockBalanceRepository.Verify(x => x.BeginTransaction(), Times.Once());
            _mockBalanceRepository.Verify(x => x.Commit(), Times.Once());
            _mockBalanceRepository.Verify(x => x.Update(balance), Times.Once());
        }


        [Test]
        public void TestGenerateBalanceWithInterestWithNegativeTotal()
        {
            //Input
            var balance = BalanceFactory.Create(x =>
            {
                x.Total = -10.00m;
                x.Charges = new List<ShortEntryDTO>();
            });

            var date = DateTime.Now;


            //behavior
            _mockBalanceRepository.Setup(m => m.BeginTransaction())
                                  .Verifiable();
            _mockBalanceRepository.Setup(m => m.Commit())
                                  .Verifiable();
            _mockBalanceRepository.Setup(m => m.Update(balance))
                                  .Verifiable();

            _mockInterestRepository.Setup(m => m.Save(It.IsAny<Interest>()));


            //test
            _service.GenerateBalanceWithInterest(balance, date);


            //assert
            Assert.AreEqual(balance.Closed, true);
            //Assert.AreEqual(-10.83m, balance.Total);

            _mockBalanceRepository.Verify(x => x.BeginTransaction(), Times.Once());
            _mockBalanceRepository.Verify(x => x.Commit(), Times.Once());
            _mockBalanceRepository.Verify(x => x.Update(balance), Times.Once());
        }


    }

}
