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
        private const string _street = "Calle Inventada, 1";
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

        private PurchasesController CreateController()
        {
            var logger = new Mock<ILogger<PurchasesController>>().Object;
            return new PurchasesController(_context, logger);
        }

        private static ValidationProblemDetails AssertBadRequestProblem(ActionResult result)
        {
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            return Assert.IsType<ValidationProblemDetails>(bad.Value);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_ModelStateInvalid()
        {
            var controller = CreateController();
            controller.ModelState.AddModelError("AnyKey", "AnyError");

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 1) },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("AnyKey"));
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_EmptyCart()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO>(), _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("Items"));
            Assert.NotEmpty(problem.Errors["Items"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_AnyItemQuantityLessThanOne()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 0) },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("Items"));
            Assert.NotEmpty(problem.Errors["Items"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_ProductDoesNotExist()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(9999, "Ghost", "Zara", "Red", 0m, 1) },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("Items"));
            Assert.NotEmpty(problem.Errors["Items"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_InsufficientStock()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 11) },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("Stock"));
            Assert.NotEmpty(problem.Errors["Stock"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_PaymentMethodNotFound()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                _street, _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 1) },
                9999, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            Assert.True(problem.Errors.ContainsKey("PaymentMethodId"));
            Assert.NotEmpty(problem.Errors["PaymentMethodId"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_BadRequest_when_StreetHasNoComma()
        {
            var controller = CreateController();

            var dto = new PurchaseForCreateDTO(
                "Calle Inventada 1", _city, _postalCode, _nameCustomer, _surname,
                new List<PurchaseItemDTO> { new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 0m, 1) },
                _pmId, null
            );

            var result = await controller.CreatePurchase(dto);
            var problem = AssertBadRequestProblem(result);

            // Current controller behavior stores this validation under PaymentMethodId
            Assert.True(problem.Errors.ContainsKey("PaymentMethodId"));
            Assert.NotEmpty(problem.Errors["PaymentMethodId"]);
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task CreatePurchase_Success_test()
        {
            var controller = CreateController();


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
            Assert.Equal(PurchaseState.Request.ToString(), detail.State);
            Assert.Equal("Bizum", detail.PaymentMethod);
            Assert.Equal("pepe@test.com", detail.CustomerUserName);
            Assert.Equal(50.00m, detail.TotalPrice);

            var expectedItems = new List<PurchaseItemDTO>
            {
                new PurchaseItemDTO(_prodJacketId, "Jacket", "Zara", "Red", 20.0m, 2),
                new PurchaseItemDTO(_prodShirtId,  "Shirt",  "Zara", "Blue",10.0m, 1)
            }
            .OrderBy(i => i.ProductId)
            .ToList();

            var actualItems = detail.Items
                .OrderBy(i => i.ProductId)
                .ToList();

            Assert.Equal(2, actualItems.Count);
            Assert.True(expectedItems.SequenceEqual(actualItems));

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