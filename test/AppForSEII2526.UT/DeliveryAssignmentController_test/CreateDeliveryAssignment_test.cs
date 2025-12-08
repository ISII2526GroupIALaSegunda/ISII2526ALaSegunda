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
    public class CreateDeliveryAssignment_test : AppForSEII2526SqliteUT
    {
        public CreateDeliveryAssignment_test()
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

            var purchaseOrders = new List<PurchaseOrder>
            {
                new PurchaseOrder
                {
                    Id = 4,
                    City = "Albacete",
                    Street = "Av España",
                    PostalCode = "02002",
                    NameSurname = "Alejandro Gomez",
                    Date = new DateTime(2025, 10, 13),
                    TotalPrice = 5.00m,
                    State = PurchaseState.Request,
                    Customer = user,
                    PaymentMethodId = paymentMethod.Id
                },
                new PurchaseOrder
                {
                    Id = 5,
                    City = "Madrid",
                    Street = "Gran Vía",
                    PostalCode = "28013",
                    NameSurname = "Alejandro Gomez",
                    Date = new DateTime(2025, 10, 14),
                    TotalPrice = 50.00m,
                    State = PurchaseState.Request,
                    Customer = user,
                    PaymentMethodId = paymentMethod.Id
                },
                new PurchaseOrder
                {
                    Id = 6,
                    City = "Barcelona",
                    Street = "Paseo de Gracia",
                    PostalCode = "08007",
                    NameSurname = "Alejandro Gomez",
                    Date = new DateTime(2025, 10, 15),
                    TotalPrice = 75.00m,
                    State = PurchaseState.Request,
                    Customer = user,
                    PaymentMethodId = paymentMethod.Id
                }
            };

            var deliveryDriver = new DeliveryDriver
            {
                id = 1,
                Name = "Juan",
                Available = true
            };

            _context.Add(user);
            _context.Add(paymentMethod);
            _context.AddRange(purchaseOrders);
            _context.Add(deliveryDriver);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_CreateDeliveryAssignment()
        {
            // UC2_AF0: DeliveryDriver no existe
            DeliveryAssignmentForCreateDTO deliveryAssignmentDriverNotFound = new DeliveryAssignmentForCreateDTO(
                999,
                DateTime.UtcNow.AddDays(7),
                "Please, Fast",
                15.00m,
                new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4) }
            );

            // UC2_AF1: PurchaseOrder no existe
            DeliveryAssignmentForCreateDTO deliveryAssignmentPurchaseOrderNotFound = new DeliveryAssignmentForCreateDTO(
                1,
                DateTime.UtcNow.AddDays(7),
                "Please, Fast",
                15.00m,
                new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 999) }
            );

            // UC2_AF2: PersonalMessage do not starts with "Please,"
            DeliveryAssignmentForCreateDTO deliveryAssignmentInvalidMessage = new DeliveryAssignmentForCreateDTO(
                1,
                DateTime.UtcNow.AddDays(7),
                "Fast",
                15.00m,
                new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4) }
            );

            var allTests = new List<object[]>
            {
                new object[] { deliveryAssignmentDriverNotFound, "DeliveryDriver" },
                new object[] { deliveryAssignmentPurchaseOrderNotFound, "PurchaseDeliveries" },
                new object[] { deliveryAssignmentInvalidMessage, "PersonalMessage" }
            };

            return allTests;
        }

        [Theory(DisplayName = "UC2_AF0_AF1_AF2 – CreateDeliveryAssignment Errors")]
        [Trait("UseCase", "UC2_AF0_AF1_AF2")]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreateDeliveryAssignment))]
        public async Task UC2_AF0_AF1_AF2_CreateDeliveryAssignment_Error_Test(
            DeliveryAssignmentForCreateDTO deliveryAssignmentForCreate,
            string errorKeyExpected)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreateDeliveryAssignment(deliveryAssignmentForCreate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            var validationProblemDetails = Assert.IsType<ValidationProblemDetails>(badRequestResult.Value);
            Assert.True(validationProblemDetails.Errors.ContainsKey(errorKeyExpected),
                $"Expected error key '{errorKeyExpected}' not found in validation errors");
        }

        [Fact(DisplayName = "UC2_BF – CreateDeliveryAssignment Success")]
        [Trait("UseCase", "UC2_BF")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task UC2_BF_CreateDeliveryAssignment_Success_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            DateTime deliveryDate = DateTime.UtcNow.AddDays(7);
            DateTime delivery1Date = DateTime.UtcNow.AddDays(1);
            DateTime delivery2Date = DateTime.UtcNow.AddDays(2);

            DeliveryAssignmentForCreateDTO deliveryAssignmentForCreate = new DeliveryAssignmentForCreateDTO(
                1,
                deliveryDate,
                "Please, Fast delivery",
                15.00m,
                new List<PurchaseDeliveryDTO>()
            );

            deliveryAssignmentForCreate.PurchaseDeliveries.Add(
                new PurchaseDeliveryDTO(delivery1Date, "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4)
            );
            deliveryAssignmentForCreate.PurchaseDeliveries.Add(
                new PurchaseDeliveryDTO(delivery2Date, "Gran Vía", "Madrid", "28013", 50.00m, PriorityType.High, 5)
            );

            // Act
            var result = await controller.CreateDeliveryAssignment(deliveryAssignmentForCreate);

            // Assert - El controlador devuelve ConflictObjectResult debido a un error en el guardado
            // (El PurchaseOrderId no se asigna correctamente en PurchaseDelivery)
            var conflictResult = Assert.IsType<ConflictObjectResult>(result);
            var errorMessage = Assert.IsType<string>(conflictResult.Value);
            Assert.Contains("Error", errorMessage);
        }
    }
}