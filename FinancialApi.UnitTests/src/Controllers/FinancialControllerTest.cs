using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FinancialApi.Controllers;
using FinancialApi.Models.Entity;
using FinancialApi.Models.Response;
using FinancialApi.Services;
using FinancialApiUnitTests.src.Factory;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace FinancialApi.UnitTests.Controllers
{
    public class FinancialControllerTest
    {

        private FinancialController mockController(Base payResult = null, Base receiveResult = null){
            if (payResult == null)
                payResult = new Errors();

            if (receiveResult == null)
                receiveResult = new Errors();

            // Mock
            var mockPaymentService = new Mock<IPaymentService>();
            mockPaymentService.Setup(service => service.Pay(It.IsAny<Payment>())).Returns(Task.FromResult<Base>(payResult));

            // Mock
            var mockReceiptService = new Mock<IReceiptService>();
            mockReceiptService.Setup(service => service.Receive(It.IsAny<Receipt>())).Returns(Task.FromResult<Base>(receiveResult));

            return new FinancialController(mockPaymentService.Object,
                                                     mockReceiptService.Object);
        }


        [Test]
        public async Task Index_ReturnsAViewResult_WithAListOfBrainstormSessions()
        {

            // initializaController
            var controller = mockController();

            // input (subject)
            var payment = PaymentFactory.Build(p => p.Description = "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");

            // output (result)
            var result = await controller.Payment(payment);

            // Assert one
            Assert.IsInstanceOf<BadRequestObjectResult>(result);

            //Assert two
            var badRequestResult = (BadRequestObjectResult)result;
            Assert.IsInstanceOf<Errors>(badRequestResult.Value);

            //Assert three
            var errors = (Errors) badRequestResult.Value;
            Assert.AreEqual(1, errors.Details.Count);
        }
    }
}