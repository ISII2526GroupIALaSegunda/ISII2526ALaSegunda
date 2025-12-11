using System.Linq;
using System.Threading.Tasks;
using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.DeliveryAssignmentDTOs;
using AppForSEII2526.API.DTOs.PurchaseDeliveryDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.DeliveryAssignmentController_test
{
    public class GetDeliveryAssignment_test : AppForSEII2526SqliteUT
    {
        public GetDeliveryAssignment_test()
        {
            var user = new ApplicationUser
            {
                Id = "1",
                Name = "Alejandro",
                Surname = "Gómez",
                Address = "Av. España",
                AccountCreationDate = DateTime.UtcNow,
                UserName = "alejandro",
                Email = "alejandro@test.es"
            };

            var paymentMethod = new PayPal
            {
                Id = 1,
                User = user,
                Email = "alejandro@test.es",
            };

            var purchaseOrder = new PurchaseOrder
            {
                Id = 1,
                City = "Albacete",
                Street = "Av España",
                PostalCode = "02002",
                NameSurname = "Alejandro Gomez",
                Date = new DateTime(2025, 10, 13),
                TotalPrice = 25.00m,
                State = PurchaseState.Request,
                Customer = user,
                PaymentMethodId = paymentMethod.Id
            };

            var deliveryDriver = new DeliveryDriver
            {
                id = 1,
                Name = "Juan",
                Available = true
            };

            var deliveryAssignment = new DeliveryAssignment
            {
                Id = 1,
                DeliveryMan = deliveryDriver,
                DeliveryAssignmentDone = new DateTime(2025, 11, 20),
                PersonalMessage = "Handle with care",
                ExtraReward = 10.00m,
                PurchaseDeliveries = new List<PurchaseDelivery>()
            };

            var purchaseDelivery = new PurchaseDelivery
            {
                DeliveryAssignmentId = 1,
                PurchaseOrderId = 1,
                Date = new DateTime(2025, 11, 18),
                Priority = PriorityType.High,
                DeliveryAssignment = deliveryAssignment,
                PurchaseOrder = purchaseOrder
            };

            deliveryAssignment.PurchaseDeliveries.Add(purchaseDelivery);

            _context.Add(user);
            _context.Add(paymentMethod);
            _context.Add(purchaseOrder);
            _context.Add(deliveryDriver);
            _context.Add(deliveryAssignment);
            _context.Add(purchaseDelivery);
            _context.SaveChanges();
        }

        [Fact(DisplayName = "UC3_AF0 – GetDeliveryAssignment Not Found")]
        [Trait("UseCase", "UC3_AF0")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task UC3_AF0_GetDeliveryAssignment_NotFound_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetDeliveryAssignment(999);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Null(okResult.Value);
        }

        [Fact(DisplayName = "UC3_BF – GetDeliveryAssignment Success")]
        [Trait("UseCase", "UC3_BF")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task UC3_BF_GetDeliveryAssignment_Success_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            var expectedDeliveryAssignment = new DeliveryAssignmentForDetailDTO(
                1,
                "Juan",
                new DateTime(2025, 11, 20),
                "Handle with care",
                10.00m,
                new List<PurchaseDeliveryDTO>()
            );

            expectedDeliveryAssignment.PurchaseDeliveries.Add(
                new PurchaseDeliveryDTO(
                    new DateTime(2025, 11, 18),
                    "Av España",
                    "Albacete",
                    "02002",
                    25.00m,
                    PriorityType.High,
                    1
                )
            );

            // Act
            var result = await controller.GetDeliveryAssignment(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualDeliveryAssignment = Assert.IsType<DeliveryAssignmentForDetailDTO>(okResult.Value);
            Assert.Equal(expectedDeliveryAssignment, actualDeliveryAssignment);
        }
    }
}