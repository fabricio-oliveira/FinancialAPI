
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
    public class PaymentServiceTest
    {
        PaymentService _service;

        Mock<IPaymentQueue> _mockPaymentQueue;
        Mock<IErrorQueue> _mockErrorQueue;

        Mock<IBalanceRepository> _mockBalanceRepository;
        Mock<IAccountRepository> _mockAccountRepository;
        Mock<IEntryRepository> _mockEntryRepository;

        [SetUp]
        public void Setup()
        {
            _mockPaymentQueue = new Mock<IPaymentQueue>();
            _mockErrorQueue = new Mock<IErrorQueue>();

            _mockBalanceRepository = new Mock<IBalanceRepository>();
            _mockAccountRepository = new Mock<IAccountRepository>();
            _mockEntryRepository = new Mock<IEntryRepository>();

            _service = new PaymentService(_mockPaymentQueue.Object,
                                          _mockErrorQueue.Object,
                                          _mockBalanceRepository.Object,
                                          _mockAccountRepository.Object,
                                          _mockEntryRepository.Object);
        }

        [TestCase("100.00", "0.00", "300.00")]
        [TestCase("100.00", "0.00", "0.00")]
        [TestCase("100.00", "0.00", "-19900.00")]
        [TestCase("100.00", "100.00", "-19800.00")]
        public void TestEnqueToPayOk(decimal entryVal, decimal charges, decimal saldo)
        {
            //Input
            var entry = EntryFactory.Build(x => { x.Type = "payment"; x.FinancialCharges = charges; x.Value = entryVal; });

            //behavior
            var account = AccountFactory.Build();
            _mockAccountRepository.Setup(m => m.FindOrCreate(It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>()))
                                  .Returns(account);

            var balance = BalanceFactory.Build(x => x.Total = saldo);
            _mockBalanceRepository.Setup(m => m.LastByOrDefault(It.IsAny<Account>()))
                                  .Returns(balance);

            _mockPaymentQueue.Setup(m => m.Enqueue(entry, null)).Verifiable();

            //It.IsAny<Entry>(

            //test
            var val = _service.EnqueueToPay(entry);


            //assert
            Assert.IsInstanceOf<OkDTO>(val.Result);
            Assert.AreEqual(entry.UUID, ((OkDTO)val.Result).UUID);
        }

        [TestCase("100.00", "0.00", "-20000.00")]
        [TestCase("100.00", "0.00", "-20000.00")]
        [TestCase("100.00", "0.00", "-19901.00")]
        [TestCase("100.00", "100.00", "-20000.00")]
        public void TestEnqueToPayFail(decimal entryVal, decimal charges, decimal saldo)
        {
            //Input
            var entry = EntryFactory.Build(x => { x.Type = "payment"; x.FinancialCharges = charges; x.Value = entryVal; });

            //behavior
            var account = AccountFactory.Build();
            _mockAccountRepository.Setup(m => m.FindOrCreate(It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>(),
                                                             It.IsAny<string>()))
                                  .Returns(account);

            var balance = BalanceFactory.Build(x => x.Total = saldo);
            _mockBalanceRepository.Setup(m => m.LastByOrDefault(It.IsAny<Account>()))
                                  .Returns(balance);

            _mockPaymentQueue.Setup(m => m.Enqueue(entry, null)).Verifiable();

            //test
            var val = _service.EnqueueToPay(entry);


            //assert
            Assert.IsInstanceOf<ErrorsDTO>(val.Result);
            Assert.AreSame("Account don't have especial limit", ((ErrorsDTO)val.Result).Details["valor_do_lancamento"][0]);
        }
    }
}
