
using System;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Repositories;
using FinancialApi.Services;
using FinancialApiUnitTests.Factory;
using Moq;
using NUnit.Framework;

namespace FinancialApiUnitTests.src.services
{
    public class ReceiptServiceTest
    {
        ReceiptService _service;

        Mock<IReceiptQueue> _mockReceiptQueue;

        Mock<IBalanceRepository> _mockBalanceRepository;
        Mock<IAccountRepository> _mockAccountRepository;
        Mock<IEntryRepository> _mockEntryRepository;

        [SetUp]
        public void Setup()
        {
            _mockReceiptQueue = new Mock<IReceiptQueue>();

            _mockBalanceRepository = new Mock<IBalanceRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockEntryRepository = new Mock<IEntryRepository>();

            _service = new ReceiptService(_mockReceiptQueue.Object,
                                          _mockBalanceRepository.Object,
                                          _mockAccountRepository.Object,
                                          _mockEntryRepository.Object);
        }

        [TestCase("100.00", "0.00", "300.00")]
        [TestCase("100.00", "0.00", "0.00")]
        [TestCase("100.00", "0.00", "-19900.00")]
        [TestCase("100.00", "100.00", "-19800.00")]
        public void TestEnqueToReceiptOk(decimal entryVal, decimal charges, decimal saldo)
        {
            //Input
            var entry = EntryFactory.Build(x => { x.Type = "receipt"; x.FinancialCharges = charges; x.Value = entryVal; });

            //behavior
            var account = AccountFactory.Build();
            _mockAccountRepository.Setup(m => m.FindOrCreateBy(It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>()))
                                  .Returns(account);

            var balance = BalanceFactory.Build(x => x.Total = saldo);
            _mockBalanceRepository.Setup(m => m.LastByOrDefault(It.IsAny<Account>()))
                                  .Returns(balance);

            _mockReceiptQueue.Setup(m => m.Enqueue(entry, null)).Verifiable();

            //It.IsAny<Entry>(

            //test
            var val = _service.EnqueueToReceive(entry);


            //assert
            Assert.IsInstanceOf<OkDTO>(val.Result);
            Assert.AreEqual(entry.UUID, ((OkDTO)val.Result).UUID);
            _mockReceiptQueue.Verify(x => x.Enqueue(entry, null), Times.Once());
        }


        [TestCase("100.00", "0.00", "300.00")]
        public void TestReceiptSucessul(decimal entryVal, decimal charges, decimal saldo)
        {
            //Input
            var entry = EntryFactory.Build(x =>
            {
                x.Type = "receipt";
                x.Value = entryVal;
                x.FinancialCharges = charges;
            });

            //behavior
            var account = AccountFactory.Build();
            _mockAccountRepository.Setup(m => m.FindOrCreateBy(It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>()))
                                  .Returns(account);

            var balance = BalanceFactory.Build(x => x.Total = saldo);
            _mockBalanceRepository.Setup(m => m.FindOrCreateBy(account, It.IsAny<DateTime>()))
                                  .Returns(balance);

            _mockBalanceRepository.Setup(m => m.LastByOrDefault(It.IsAny<Account>()))
                                  .Returns(balance);

            _mockEntryRepository.Setup(m => m.BeginTransaction()).Verifiable();
            _mockEntryRepository.Setup(m => m.Commit()).Verifiable();

            //test
            var val = _service.Receive(entry);

            //assert
            Assert.IsInstanceOf<OkDTO>(val.Result);
            Assert.AreEqual(entry.UUID, ((OkDTO)val.Result).UUID);
            _mockEntryRepository.Verify(x => x.Save(entry), Times.Once());
            _mockBalanceRepository.Verify(x => x.Update(balance), Times.Once());
            Assert.AreEqual(0, balance.Outputs.Count);
            Assert.AreEqual(1, balance.Charges.Count);
            Assert.AreEqual(1, balance.Inputs.Count);

        }

    }
}
