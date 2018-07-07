using System.Threading.Tasks;
using FinancialApi.Controllers;
using FinancialApi.Models.Entity;
using FinancialApi.Models.DTO;
using FinancialApi.Services;
using FinancialApiUnitTests.src.Factory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using FinancialApiUnitTests.src.support;

namespace FinancialApi.UnitTests.Controllers
{
    public class FinancialControllerTest
    {

        private FinancialController mockController(IBaseDTO payResult = null, IBaseDTO receiveResult = null){
            if (payResult == null)
                payResult = new OkDTO("x");

            if (receiveResult == null)
                receiveResult = new OkDTO("y");

            // Mock
            var mockPaymentService = new Mock<IPaymentService>();
            mockPaymentService.Setup(service => service.Pay(It.IsAny<Payment>())).Returns(Task.FromResult<IBaseDTO>(payResult));

            // Mock
            var mockReceiptService = new Mock<IReceiptService>();
            mockReceiptService.Setup(service => service.Receive(It.IsAny<Receipt>())).Returns(Task.FromResult<IBaseDTO>(receiveResult));

            return new FinancialController(mockPaymentService.Object,
                                                     mockReceiptService.Object);
        }


        [Test]
        public async Task Pay_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = mockController();

            // input (subject)
            var payment = PaymentFactory.Build();

            // output (result)
            var result = await controller.Payment(payment);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (OkDTO) ResponseResult.Value;
            Assert.AreEqual("x", bodyResult.UUID);
        }

        [Test]
        public async Task Receipt_ReturnsAOkObjectResult_WithAUUID()
        {

            // initializaController
            var controller = mockController();

            // input (subject)
            var receipt = ReceiptFactory.Build();

            // output (result)
            var result = await controller.Receipt(receipt);

            // Assert one
            Assert.IsInstanceOf<OkObjectResult>(result);

            //Assert two
            var ResponseResult = (OkObjectResult)result;
            Assert.IsInstanceOf<OkDTO>(ResponseResult.Value);

            //Assert three
            var bodyResult = (OkDTO)ResponseResult.Value;
            Assert.AreEqual("y", bodyResult.UUID);
        }
    }
}