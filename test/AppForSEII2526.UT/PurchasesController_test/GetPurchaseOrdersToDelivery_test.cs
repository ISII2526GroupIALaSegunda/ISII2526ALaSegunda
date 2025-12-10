using System.Linq;
using System.Threading.Tasks;
using AppForMovies.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;


namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchaseOrdersToDelivery_test : AppForMovies4SqliteUT
    {
        public GetPurchaseOrdersToDelivery_test()
        {
            var user = new ApplicationUser
            {
                Id = "2",
                Name = "Alejandro",
                Surname = "Gómez",
                Address = "Av. España",
                AccountCreationDate = DateTime.UtcNow,
                UserName = "alejandro",
                Email = "alejandro@test.es"
            };

            var paymentMethod = new PayPal { User = user, Email = "alejandro@test.es" };

            var purchaseOrders = new List<PurchaseOrder>
            {
                new PurchaseOrder
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
                    PaymentMethodId = 1
                },
                new PurchaseOrder
                {
                    Id = 2,
                    City = "Madrid",
                    Street = "Gran Via",
                    PostalCode = "28013",
                    NameSurname = "Alejandro Gomez",
                    Date = new DateTime(2025, 10, 14),
                    TotalPrice = 50.00m,
                    State = PurchaseState.Request,
                    Customer = user,
                    PaymentMethodId = 1
                },
                new PurchaseOrder
                {
                    Id = 3,
                    City = "Albacete",
                    Street = "Av España",
                    PostalCode = "02002",
                    NameSurname = "Alejandro Gomez",
                    Date = new DateTime(2025, 10, 13),
                    Rating = 5,
                    TotalPrice = 5.00m,
                    State = PurchaseState.Delivery,
                    Customer = user,
                    PaymentMethodId = 1
                }
            };

            _context.Users.Add(user);
            _context.Add(paymentMethod);
            _context.AddRange(purchaseOrders);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetPurchaseOrdersToDelivery_OK()
        {
            var purchaseOrderDTOs = new List<PurchaseOrderForDeliveryDTO>
            {
                new PurchaseOrderForDeliveryDTO(1, new DateTime(2025, 10, 13), 25.00m, "Albacete", "Av España", "02002"),
                new PurchaseOrderForDeliveryDTO(2, new DateTime(2025, 10, 14), 50.00m, "Madrid", "Gran Via", "28013")
            };

            var expectedAll = new List<PurchaseOrderForDeliveryDTO> { purchaseOrderDTOs[0], purchaseOrderDTOs[1] };
            var expectedByPostalCode = new List<PurchaseOrderForDeliveryDTO> { purchaseOrderDTOs[0] };
            var expectedByPrice = new List<PurchaseOrderForDeliveryDTO> { purchaseOrderDTOs[1] };

            return new List<object[]>
            {
                new object[] { null,     null,    expectedAll },              // UC1_BF
                new object[] { "02002",  null,    expectedByPostalCode },     // UC1_AF0
                new object[] { null,     50.00m,  expectedByPrice }           // UC1_AF0
            };
        }

        [Theory(DisplayName = "UC1_BF_AF0 – GetPurchaseOrdersToDelivery")]
        [Trait("UseCase", "UC1_BF_AF0")]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPurchaseOrdersToDelivery_OK))]
        public async Task UC1_BF_AF0_GetPurchaseOrdersToDelivery_Test(
            string? postalcode,
            decimal? totalprice,
            List<PurchaseOrderForDeliveryDTO> expectedPurchaseOrders)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchaseOrdersController>>();
            var controller = new PurchaseOrdersController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetPurchaseOrdersToDelivery(postalcode, totalprice);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualPurchaseOrders = Assert.IsType<List<PurchaseOrderForDeliveryDTO>>(okResult.Value);
            Assert.Equal(expectedPurchaseOrders, actualPurchaseOrders);
        }

        [Fact(DisplayName = "UC1_AF1 – GetPurchaseOrdersToDelivery No Results")]
        [Trait("UseCase", "UC1_AF1")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task UC1_AF1_GetPurchaseOrdersToDelivery_NoResults_Test()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchaseOrdersController>>();
            var controller = new PurchaseOrdersController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetPurchaseOrdersToDelivery("99999", null);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            var errorMessage = Assert.IsType<string>(badRequest.Value);
            Assert.Equal("No purchase orders in state 'Request' found with requested data", errorMessage);
        }
    }
}