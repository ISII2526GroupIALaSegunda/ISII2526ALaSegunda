using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs_PurchaseProductDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.PurchaseProductsController_test
{
    public class GetPurchaseProductsForReturning_test : AppForSEII2526SqliteUT // o AppForMovies4SqliteUT
    {
        public GetPurchaseProductsForReturning_test()
        {
            // ==== Seed de datos siguiendo la idea de tu script SQL ====

            // 1) Usuarios
            var user1 = new ApplicationUser
            {
                Id = "1",
                Name = "User1",

            };

            var user2 = new ApplicationUser
            {
                Id = "2",
                Name = "User2",

            };

            // 2) Brands
            var brand1 = new Brand
            {
                Id = 1,
                Name = "Brand1",
                Location = "Location1"
            };

            var brand2 = new Brand
            {
                Id = 2,
                Name = "Brand2",
                Location = "Location2"
            };

            // 3) Products (solo necesitamos los que se usan)
            var product2 = new Product
            {
                ProductId = 2,
                Name = "ProductCok",
                Description = "Second Product",
                Colour = "Red",
                Price = 50m,
                Stock = 20,
                IsReturnable = true,
                Brand = brand2
            };

            var product3 = new Product
            {
                ProductId = 3,
                Name = "Product3",
                Description = "Third Product",
                Colour = "Blue",
                Price = 75m,
                Stock = 15,
                IsReturnable = true, // también retornable, pero lo marcaremos como ya devuelto
                Brand = brand1
            };

            // 4) PurchaseOrders
            var po1 = new PurchaseOrder
            {
                Id = 1,
                City = "City1",
                Street = "Street1",
                PostalCode = "Postal1",
                NameSurname = "Name1Surname1",
                Description = "Description1",
                Date = new DateTime(2025, 11, 29),
                Rating = 3,
                TotalPrice = 200m,
                State = 1,
                Customer = user1,
                PaymentMethodId = 2, // nos da igual para este controller
                PurchaseProducts = new List<PurchaseProduct>()
            };

            var po3 = new PurchaseOrder
            {
                Id = 3,
                City = "City2",
                Street = "Street2",
                PostalCode = "Postal2",
                NameSurname = "Name2Surname2",
                Description = "Description2",
                Date = new DateTime(2025, 11, 30),
                Rating = 4,
                TotalPrice = 250m,
                State = 2,
                Customer = user2,
                PaymentMethodId = 2,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            // 5) PurchaseProducts (clave compuesta PurchaseOrderId + ProductId)
            var pp_po1_prod2 = new PurchaseProduct
            {
                PurchaseOrder = po1,
                PurchaseOrderId = po1.Id,
                Product = product2,
                ProductId = product2.ProductId,
                Price = 200m,
                Quantity = 10
            };

            var pp_po3_prod3 = new PurchaseProduct
            {
                PurchaseOrder = po3,
                PurchaseOrderId = po3.Id,
                Product = product3,
                ProductId = product3.ProductId,
                Price = 250m,
                Quantity = 11
            };

            po1.PurchaseProducts.Add(pp_po1_prod2);
            po3.PurchaseProducts.Add(pp_po3_prod3);

            // 6) ReturnPurchaseOrders y ReturnProducts (solo para marcar productos ya devueltos)
            var rpo1 = new ReturnPurchaseOrder
            {
                Id = 6,
                Name = "Return1",
                Date = new DateTime(2025, 10, 15),
                MoneyToReturn = 50,
                NewTotalPrice = 100m,
                Rating = 3,
                TotalPrice = 200m,
                PaymentMethodId = 2,
                Customer = user1,
                ReturnProducts = new List<ReturnProduct>()
            };

            var rpo2 = new ReturnPurchaseOrder
            {
                Id = 7,
                Name = "Return2",
                Date = new DateTime(2025, 10, 16),
                MoneyToReturn = 100,
                NewTotalPrice = 150m,
                Rating = 4,
                TotalPrice = 250m,
                PaymentMethodId = 2,
                Customer = user2,
                ReturnProducts = new List<ReturnProduct>()
            };

            // ReturnProduct asociado a (PO3, Product3) → para que NO salga en el GET
            var rp_po3_prod3 = new ReturnProduct
            {
                Id = 45,
                Quantity = 25,
                Reason = "Second Reason",
                ReturnPurchaseOrder = rpo2,
                PurchaseOrderId = po3.Id,
                ProductId = product3.ProductId,
                PurchaseProduct = pp_po3_prod3
            };

            rpo2.ReturnProducts.Add(rp_po3_prod3);

            // ==== Añadimos todo al contexto ====
            _context.Users.AddRange(user1, user2);
            _context.Brands.AddRange(brand1, brand2);
            _context.Products.AddRange(product2, product3);
            _context.PurchaseOrders.AddRange(po1, po3);
            _context.PurchaseProducts.AddRange(pp_po1_prod2, pp_po3_prod3);
            _context.ReturnPurchaseOrders.AddRange(rpo1, rpo2);
            _context.ReturnProducts.Add(rp_po3_prod3);

            _context.SaveChanges();
        }

        // ==================== MemberData ====================

        public static IEnumerable<object[]> TestCasesFor_GetPurchaseProductsForReturning_OK()
        {
            // En este escenario:
            // - TheUser1 tiene PurchaseOrder 1 con ProductId = 2 (returnable, sin ReturnProduct) → DEBE salir.
            // - TheUser2 tiene PurchaseOrder 3 con ProductId = 3 (returnable, PERO con ReturnProduct) → NO debe salir.

            var expectedForUser1 = new List<PurchaseProductsForReturningDTO>()
            {
                new PurchaseProductsForReturningDTO(
                    2,              // ProductId
                    "ProductCok",   // Product.Name
                    10,             // Quantity (del PurchaseProduct)
                    "Brand2",       // Brand.Name
                    "Location2",    // Brand.Location
                    true            // IsReturnable
                )
            };

            var expectedForUser2 = new List<PurchaseProductsForReturningDTO>()
            {
                // Esperamos lista vacía: su único producto ya tiene ReturnProduct
            };

            var allTests = new List<object[]>
            {
                // productName, quantity, userName, expectedList
                new object[] { null, 0, "TheUser1", expectedForUser1 },
                new object[] { null, 0, "TheUser2", expectedForUser2 }
            };

            return allTests;
        }

        // ==================== Theory ====================

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetPurchaseProductsForReturning_OK))]
        public async Task GetPurchaseProductsForReturning_filter_test(
            string? productName,
            int quantity,
            string userName,
            List<PurchaseProductsForReturningDTO> expectedProducts)
        {
            // Arrange
            var controller = new PurchaseProductsController(_context, null);

            // Act
            var result = await controller.GetPurchaseProductsForReturning(productName, quantity, userName);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualProducts = Assert.IsType<List<PurchaseProductsForReturningDTO>>(okResult.Value);

            Assert.Equal(expectedProducts, actualProducts);
        }
    }
}

