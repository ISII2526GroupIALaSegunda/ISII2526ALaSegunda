using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.PurchaseOrderDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class GetPurchase_test : AppForSEII2526SqliteUT
    {
        private PurchasesController CreateController()
        {
            return new PurchasesController(_context, new Mock<ILogger<PurchasesController>>().Object);
        }

        [Fact]
        [Trait("Database", "SQLite:Memory")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_NotFound_test()
        {
            var ctrl = CreateController();

            var result = await ctrl.GetPurchase(0);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        [Trait("Database", "SQLite:Memory")]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task GetPurchase_Found_test()
        {

            var brand = new Brand { Id = 1, Name = "Zara", Location = "Madrid" };
            var prod1 = new Product { ProductId = 1, Name = "Jacket", Colour = "Red", Price = 20.0m, Stock = 10, IsReturnable = true, Brand = brand };
            var prod2 = new Product { ProductId = 2, Name = "Shirt", Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brand };
            _context.AddRange(brand, prod1, prod2);

            var user = new ApplicationUser
            {
                Id = "U1",
                Name = "Pepe",
                Surname = "Pérez",
                Address = "C/ Prueba 1",
                AccountCreationDate = System.DateTime.UtcNow,
                UserName = "pepe@test.com",
                Email = "pepe@test.com"
            };
            var pm = new Bizum { User = user, TelephoneNumber = "+34600111222" };
            _context.AddRange(user, pm);
            await _context.SaveChangesAsync();

            var order = new PurchaseOrder
            {
                City = "Albacete",
                Street = "Calle Inventada",
                PostalCode = "02001",
                NameSurname = "Pepe Pérez",
                Date = System.DateTime.Now,
                State = PurchaseState.Request,
                Customer = user,
                PaymentMethodId = pm.Id,
                TotalPrice = 0m
            };
            _context.PurchaseOrders.Add(order);
            await _context.SaveChangesAsync();

            var line1 = new PurchaseProduct { PurchaseOrderId = order.Id, ProductId = prod1.ProductId, Price = prod1.Price, Quantity = 2 };
            var line2 = new PurchaseProduct { PurchaseOrderId = order.Id, ProductId = prod2.ProductId, Price = prod2.Price, Quantity = 1 };
            _context.PurchaseProducts.AddRange(line1, line2);
            await _context.SaveChangesAsync();

            order.TotalPrice = line1.Price * line1.Quantity + line2.Price * line2.Quantity;
            _context.PurchaseOrders.Update(order);
            await _context.SaveChangesAsync();

            var ctrl = CreateController();

            var result = await ctrl.GetPurchase(order.Id);

            var ok = Assert.IsType<OkObjectResult>(result);
            var dto = Assert.IsType<PurchaseForDetailDTO>(ok.Value);

            Assert.Equal(order.Id, dto.Id);
            Assert.Equal(order.TotalPrice, dto.TotalPrice);
            Assert.Equal(order.Date, dto.Date);
            Assert.Equal("Calle Inventada", dto.Street);
            Assert.Equal("Albacete", dto.City);
            Assert.Equal("02001", dto.PostalCode);
            Assert.Equal("Pepe Pérez", dto.NameSurname);

            Assert.Equal(PurchaseState.Request.ToString(), dto.State);
            Assert.Equal("Bizum", dto.PaymentMethod);

            Assert.Equal("pepe@test.com", dto.CustomerUserName);

            var expectedItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(prod1.ProductId, "Jacket", "Zara", "Red", 20.0m, 2),
                new PurchaseItemDTO(prod2.ProductId, "Shirt",  "Zara", "Blue",10.0m, 1)
            }
            .OrderBy(i => i.ProductId)
            .ToList();

            var actualItems = dto.Items
                .OrderBy(i => i.ProductId)
                .ToList();

            Assert.Equal(2, actualItems.Count);
            Assert.True(expectedItems.SequenceEqual(actualItems));
        }
    }
}
