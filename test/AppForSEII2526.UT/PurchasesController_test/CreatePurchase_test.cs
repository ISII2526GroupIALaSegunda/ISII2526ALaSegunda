using System;
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
using Microsoft.AspNetCore.Http;

namespace AppForSEII2526.UT.PurchasesController_test
{
    public class CreatePurchase_test : AppForSEII2526SqliteUT
    {    
        private const string _street = "Calle Inventada 1";
        private const string _city = "Albacete";
        private const string _postalCode = "02001";
        private const string _nameCustomer = "Pepe";
        private const string _surname = "Pérez";

        private int _pmId;         
        private int _prodJacketId;  
        private int _prodShirtId;   

        public CreatePurchase_test()
        {
            var brand = new Brand { Id = 1, Name = "Zara", Location = "Madrid" };
            var p1 = new Product { ProductId = 1, Name = "Jacket", Colour = "Red", Price = 20.0m, Stock = 10, IsReturnable = true, Brand = brand };
            var p2 = new Product { ProductId = 2, Name = "Shirt", Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brand };
            _context.AddRange(brand, p1, p2);

            var user = new ApplicationUser
            {
                Id = "U1",
                Name = _nameCustomer,
                Surname = _surname,
                Address = "C/ X",
                AccountCreationDate = DateTime.UtcNow,
                UserName = "pepe@test.com",
                Email = "pepe@test.com"
            };
            var pm = new Bizum { User = user, TelephoneNumber = "+34600111222" };
            _context.AddRange(user, pm);

            _context.SaveChanges();

            _pmId = pm.Id;
            _prodJacketId = p1.ProductId;
            _prodShirtId = p2.ProductId;
        }

        public static IEnumerable<object[]> TestCasesFor_CreatePurchase_Errors()
        {

            // 1) Carrito vacío
            var emptyCart = new Func<int, PurchaseForCreateDTO>(pmId => new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO>(), pmId, null
            ));

            // 2) Cantidad inválida (<1)
            var invalidQty = new Func<int, int, PurchaseForCreateDTO>((pmId, productId) => new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> {
                    new PurchaseItemDTO(productId, "Jacket", "Zara", "Red", 0m, 0) // Quantity = 0
                },
                pmId, null
            ));

            // 3) Producto inexistente
            var productNotFound = new Func<int, PurchaseForCreateDTO>(pmId => new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> {
                    new PurchaseItemDTO(9999, "Ghost", "Zara", "Red", 0m, 1)
                },
                pmId, null
            ));

            // 4) PaymentMethod inexistente
            var pmNotFound = new Func<PurchaseForCreateDTO>(() => new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> {
                    new PurchaseItemDTO(1, "Jacket", "Zara", "Red", 0m, 1)
                },
                9999, null 
            ));

            return new List<object[]>
            {
                new object[] { (Func<int, int, PurchaseForCreateDTO>) ((pmId, _) => emptyCart(pmId)), "must include at least one product" },
                new object[] { (Func<int, int, PurchaseForCreateDTO>) ((pmId, prodId) => invalidQty(pmId, prodId)), "Quantity must be >= 1" },
                new object[] { (Func<int, int, PurchaseForCreateDTO>) ((pmId, _) => productNotFound(pmId)), "Products not found" },
                new object[] { (Func<int, int, PurchaseForCreateDTO>) ((_, __) => pmNotFound()), "Payment method not found" },
            };
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_CreatePurchase_Errors))]
        public async Task CreatePurchase_Error_test(Func<int, int, PurchaseForCreateDTO> dtoFactory, string expectedErrorContains)
        {
            var logger = new Mock<ILogger<PurchasesController>>().Object;
            var controller = new PurchasesController(_context, logger);
            var dto = dtoFactory(_pmId, _prodJacketId);
            var result = await controller.CreatePurchase(dto);
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            var problem = Assert.IsType<ValidationProblemDetails>(bad.Value);


            var errorActual = problem.Errors.First().Value[0];
            Assert.Contains(expectedErrorContains, errorActual);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_Success_test()
        {
    
            var logger = new Mock<ILogger<PurchasesController>>().Object;
            var controller = new PurchasesController(_context, logger);

    
            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO>
                {
                    new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 2),
                    new PurchaseItemDTO(_prodShirtId,  "Shirt",  "Zara", "Blue",0m, 1)
                },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(PurchasesController.GetPurchase), created.ActionName);
            var detail = Assert.IsType<PurchaseForDetailDTO>(created.Value);

      
            Assert.Equal(_street, detail.Street);
            Assert.Equal(_city, detail.City);
            Assert.Equal(_postalCode, detail.PostalCode);
            Assert.Equal($"{_nameCustomer} {_surname}", detail.NameSurname);

            // Estado y método de pago
            Assert.Equal(PurchaseState.Request.ToString(), detail.State);
            Assert.Equal("Bizum", detail.PaymentMethod);

            // Usuario
            Assert.Equal("pepe@test.com", detail.CustomerUserName);

            // Total calculado (snapshot de precio)
            Assert.Equal(50.00m, detail.TotalPrice);

            Assert.Equal(2, detail.Items.Count);

            var i1 = detail.Items.Single(i => i.ProductId == _prodJacketId);
            Assert.Equal("Jacket", i1.Name);
            Assert.Equal("Zara", i1.Brand);
            Assert.Equal("Red", i1.Colour);
            Assert.Equal(20.0m, i1.UnitPrice);
            Assert.Equal(2, i1.Quantity);

            var i2 = detail.Items.Single(i => i.ProductId == _prodShirtId);
            Assert.Equal("Shirt", i2.Name);
            Assert.Equal("Zara", i2.Brand);   
            Assert.Equal("Blue", i2.Colour);
            Assert.Equal(10.0m, i2.UnitPrice);
            Assert.Equal(1, i2.Quantity);

            Assert.True(Math.Abs((DateTime.Now - detail.Date).TotalMinutes) < 2);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void CreatePurchase_Attributes_test()
        {
            var method = typeof(PurchasesController).GetMethod(nameof(PurchasesController.CreatePurchase));
            Assert.NotNull(method);

            // HttpPost
            var httpPostAttr = method!.GetCustomAttributes(typeof(HttpPostAttribute), false).FirstOrDefault();
            Assert.NotNull(httpPostAttr);

            // Route("[action]")
            var routeAttr = method.GetCustomAttributes(typeof(RouteAttribute), false).Cast<RouteAttribute>().FirstOrDefault();
            Assert.NotNull(routeAttr);
            Assert.Equal("[action]", routeAttr!.Template);

            // Parameter should have [FromBody]
            var param = method.GetParameters().FirstOrDefault();
            Assert.NotNull(param);
            var fromBodyAttr = param!.GetCustomAttributes(typeof(FromBodyAttribute), false).FirstOrDefault();
            Assert.NotNull(fromBodyAttr);

            // ProducesResponseType attributes
            var produces = method.GetCustomAttributes(typeof(ProducesResponseTypeAttribute), false).Cast<ProducesResponseTypeAttribute>().ToList();
            Assert.Contains(produces, a => a.StatusCode == StatusCodes.Status201Created && a.Type == typeof(PurchaseForDetailDTO));
            Assert.Contains(produces, a => a.StatusCode == StatusCodes.Status400BadRequest && a.Type == typeof(ValidationProblemDetails));
            Assert.Contains(produces, a => a.StatusCode == StatusCodes.Status409Conflict && a.Type == typeof(string));
        }
    }
}
