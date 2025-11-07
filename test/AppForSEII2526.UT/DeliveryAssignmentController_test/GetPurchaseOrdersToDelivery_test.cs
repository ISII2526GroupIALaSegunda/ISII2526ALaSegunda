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


namespace AppForSEII2526.UT.DeliveryAssignmentController_test
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

        public static IEnumerable<object[]> TestCasesFor_GetPurchaseOrdersToDelivery()
        {
            var expectedAll = new List<PurchaseOrderForDeliveryDTO>
            {
                new PurchaseOrderForDeliveryDTO(1, new DateTime(2025, 10, 13), 25.00m, "Albacete", "Av España", "02002"),
                new PurchaseOrderForDeliveryDTO(2, new DateTime(2025, 10, 14), 50.00m, "Madrid", "Gran Via", "28013")
            };

            var expectedByPostalCode = new List<PurchaseOrderForDeliveryDTO>
            {
                new PurchaseOrderForDeliveryDTO(1, new DateTime(2025, 10, 13), 25.00m, "Albacete", "Av España", "02002")
            };

            var expectedByPrice = new List<PurchaseOrderForDeliveryDTO>
            {
                new PurchaseOrderForDeliveryDTO(2, new DateTime(2025, 10, 14), 50.00m, "Madrid", "Gran Via", "28013")
            };

            var expectedNone = new List<PurchaseOrderForDeliveryDTO>();

            return new List<object[]>
            {
                new object[] { null,     null,    expectedAll },              // UC1_BF
                new object[] { "02002",  null,    expectedByPostalCode },     // UC1_AF0
                new object[] { null,     50.00m,  expectedByPrice },          // UC1_AF0
                new object[] { "99999",  null,    expectedNone }              // UC1_AF1
            };
        }

        [Theory(DisplayName = "UC1_BF_AF0_AF1 – GetPurchaseOrdersToDelivery")]
        [Trait("UseCase", "UC1_BF_AF0_AF1")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_GetPurchaseOrdersToDelivery))]
        public async Task UC1_BF_AF0_AF1_GetPurchaseOrdersToDelivery_Test(
            string? postalcode,
            decimal? totalprice,
            IList<PurchaseOrderForDeliveryDTO> expected)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<PurchaseOrdersController>>();
            var controller = new PurchaseOrdersController(_context, mockLogger.Object);

            // Act
            var result = await controller.GetPurchaseOrdersToDelivery(postalcode, totalprice);

            // Assert
            if (!expected.Any())
            {
                // AF1: No results found - should return BadRequest
                var badRequest = Assert.IsType<BadRequestObjectResult>(result);
                var errorMessage = Assert.IsType<string>(badRequest.Value);
                Assert.Equal("No purchase orders in state 'Request' found with requested data", errorMessage);

                // Verificar que el logger recibió un error
                mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("No purchase orders in state 'Request' found with requested data")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }
            else
            {
                // BF/AF0: Results found - should return OK
                var ok = Assert.IsType<OkObjectResult>(result);
                var actual = Assert.IsType<List<PurchaseOrderForDeliveryDTO>>(ok.Value);

                // Comparar propiedad por propiedad para evitar problemas de igualdad
                var expectedOrdered = expected.OrderBy(p => p.Id).ToList();
                var actualOrdered = actual.OrderBy(p => p.Id).ToList();

                Assert.Equal(expectedOrdered.Count, actualOrdered.Count);

                for (int i = 0; i < expectedOrdered.Count; i++)
                {
                    Assert.Equal(expectedOrdered[i].Id, actualOrdered[i].Id);
                    Assert.Equal(expectedOrdered[i].Date, actualOrdered[i].Date);
                    Assert.Equal(expectedOrdered[i].TotalPrice, actualOrdered[i].TotalPrice);
                    Assert.Equal(expectedOrdered[i].City, actualOrdered[i].City);
                    Assert.Equal(expectedOrdered[i].Street, actualOrdered[i].Street);
                    Assert.Equal(expectedOrdered[i].PostalCode, actualOrdered[i].PostalCode);
                }
            }
        }
    }
}