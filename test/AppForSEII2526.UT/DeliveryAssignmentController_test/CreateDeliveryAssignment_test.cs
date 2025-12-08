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

        [Fact(DisplayName = "UC2_BF – CreateDeliveryAssignment Success")]
        [Trait("UseCase", "UC2_BF")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        public async Task UC2_BF_CreateDeliveryAssignment_Success_Test()
        {
            // Arrange
            var validDeliveryAssignment = new DeliveryAssignmentForCreateDTO(
                deliveryDriverId: 1,
                deliveryAssignmentDone: DateTime.UtcNow.AddDays(7),
                personalMessage: "Please, Fast delivery",
                extraReward: 15.00m,
                purchaseDeliveries: new List<PurchaseDeliveryDTO>
                {
                    new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4),
                    new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(2), "Gran Vía", "Madrid", "28013", 50.00m, PriorityType.High, 5)
                }
            );

            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreateDeliveryAssignment(validDeliveryAssignment);

            // Assert
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            var deliveryAssignmentDetail = Assert.IsType<DeliveryAssignmentForDetailDTO>(createdResult.Value);
            Assert.Equal("GetDeliveryAssignment", createdResult.ActionName);
            Assert.Equal("Juan", deliveryAssignmentDetail.DeliveryDriverName);
            Assert.Equal(15.00m, deliveryAssignmentDetail.ExtraReward);
            Assert.Equal("Please, Fast delivery", deliveryAssignmentDetail.PersonalMessage);

            // Verificar que la asignación está en la BD
            var deliveryAssignmentInDb = _context.DeliveryAssignments
                .Include(da => da.PurchaseDeliveries)
                .FirstOrDefault(da => da.Id == deliveryAssignmentDetail.Id);

            Assert.NotNull(deliveryAssignmentInDb);
            Assert.Equal("Juan", deliveryAssignmentInDb.DeliveryMan.Name);
        }

        public static IEnumerable<object[]> TestCasesFor_CreateDeliveryAssignment_Error()
        {
            return new List<object[]>
            {
                // UC2_AF0: DeliveryDriver no existe
                new object[]
                {
                    new DeliveryAssignmentForCreateDTO(999, DateTime.UtcNow.AddDays(7), "Please, Fast", 15.00m,
                        new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4) }),
                    "error occurred",
                    typeof(ConflictObjectResult)
                },

                // UC2_AF1: PurchaseOrder no existe
                new object[]
                {
                    new DeliveryAssignmentForCreateDTO(1, DateTime.UtcNow.AddDays(7), "Please, Fast", 15.00m,
                        new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 999) }),
                    "PurchaseDelivery",
                    typeof(ConflictObjectResult)
                },
                // UC2_AF2: PersonalMessage do not starts with "Please,"
                new object[]
                {
                    new DeliveryAssignmentForCreateDTO(1, DateTime.UtcNow.AddDays(7), "Fast", 15.00m,
                        new List<PurchaseDeliveryDTO> { new PurchaseDeliveryDTO(DateTime.UtcNow.AddDays(1), "Av España", "Albacete", "02002", 5.00m, PriorityType.Medium, 4) }),
                    "You must start",
                    typeof(ConflictObjectResult)
                }
            };
        }

        [Theory(DisplayName = "UC2_AF0_AF1_AF2 – CreateDeliveryAssignment Errors")]
        [Trait("UseCase", "UC2_AF0_AF1")]
        [Trait("LevelTesting", "Unit Testing")]
        [Trait("Database", "WithoutFixture")]
        [MemberData(nameof(TestCasesFor_CreateDeliveryAssignment_Error))]
        public async Task UC2_AF0_AF1_CreateDeliveryAssignment_Error_Test(
            DeliveryAssignmentForCreateDTO deliveryAssignmentForCreate,
            string expectedError,
            Type expectedResultType)
        {
            // Arrange
            var mockLogger = new Mock<ILogger<DeliveryAssignmentsController>>();
            var controller = new DeliveryAssignmentsController(_context, mockLogger.Object);

            // Act
            var result = await controller.CreateDeliveryAssignment(deliveryAssignmentForCreate);

            // Assert
            Assert.IsType(expectedResultType, result);

            if (result is ConflictObjectResult conflictResult)
            {
                var errorMessage = Assert.IsType<string>(conflictResult.Value);
                Assert.Contains(expectedError, errorMessage, StringComparison.OrdinalIgnoreCase);

                // Verificar que el logger registró el error
                mockLogger.Verify(
                    x => x.Log(
                        LogLevel.Error,
                        It.IsAny<EventId>(),
                        It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Error")),
                        It.IsAny<Exception>(),
                        It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                    Times.Once);
            }
        }
    }
}