using System.Collections.Generic;
using System.Threading.Tasks;
using FinancialApi.Controllers;
using FinancialApi.Models.DTO.Response;
using FinancialApi.Models.Entity;
using FinancialApi.Services;
using FinancialApiUnitTests.Factory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FinancialApi.UnitTests.Controllers
{
    public class FinancialControllerTest
    {

        private FinancialController MockController(IBaseDTO payResult = null,
                                                   IBaseDTO receiveResult = null,
                                                   List<Balance> balancesResult = null)
        {
            if (payResult == null)
                payResult = new OkDTO("x");

            if (receiveResult == null)
                receiveResult = new OkDTO("y");

            if (balancesResult == null)
                balancesResult = new List<Balance>();

            // Mock
            var mockPaymentService = new Mock<IPaymentService>();
            mockPaymentService.Setup(service => service.EnqueueToPay(It.IsAny<Entry>())).Returns(Task.FromResult<IBaseDTO>(payResult));

            // Mock
            var mockReceiptService = new Mock<IReceiptService>();
            mockReceiptService.Setup(service => service.EnqueueToReceive(It.IsAny<Entry>())).Returns(Task.FromResult<IBaseDTO>(receiveResult));

            var mockBalanceService = new Mock<IBalanceService>();
            mockBalanceService.Setup(service => service.CashFlow(It.IsAny<Account>())).Returns(balancesResult);


            return new FinancialController(mockPaymentService.Object,
                                           mockReceiptService.Object,
                                           mockBalanceService.Object);
        }

        // Payment
        [Test]
        public async Task Pay_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = MockController();

            // input (subject)
            var payment = EntryFactory.Build();

            // test (result)
            var result = await controller.Entry(payment);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (OkDTO)ResponseResult.Value;
            Assert.AreEqual("x", bodyResult.UUID);
        }


        [Test]
        public async Task Pay_ReturnsABadRequestObjectResult_WithFailBody()
        {

            // initializaController
            var controller = MockController();
            controller.ModelState.AddModelError("Description", "some error");

            // input (subject)
            var payment = EntryFactory.Build();

            // test (result)
            var result = await controller.Entry(payment);

            // Assert one
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            //Assert two
            var ResponseResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOf<ErrorsDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (ErrorsDTO)ResponseResult.Value;
            Assert.AreEqual(1, bodyResult.Details.Keys.Count);

            Assert.AreEqual("some error", bodyResult.Details["descricao"][0]);
        }

        //Receipt
        [Test]
        public async Task Receipt_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = MockController();

            // input (subject)
            var receipt = EntryFactory.Build();

            // test (result)
            var result = await controller.Entry(receipt);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (OkDTO)ResponseResult.Value;
        }
    }
}