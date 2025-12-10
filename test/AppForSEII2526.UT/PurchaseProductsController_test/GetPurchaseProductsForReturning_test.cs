using AppForSEII2526.UT;
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs_PurchaseProductDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.UT.PurchaseProductsController_test
{
    public class GetPurchaseProductsForReturning_test : AppForSEII2526SqliteUT
    {
        public GetPurchaseProductsForReturning_test()
        {


            // 1) Users
            var user1 = new ApplicationUser
            {
                Id = "1",
                UserName = "TheUser1",
                Name = "User1",
                Surname = "Test1",
                Address = "Test Street 1",
                Email = "user1@test.com"
            };

            var user2 = new ApplicationUser
            {
                Id = "2",
                UserName = "TheUser2",
                Name = "User2",
                Surname = "Test2",
                Address = "Test Street 2",
                Email = "user2@test.com"
            };

            // 2) Brand
            var brand1 = new Brand
            {
                Id = 1,
                Name = "Brand1",
                Location = "Location1"
            };

            // 3) Products
            var productForUser1 = new Product
            {
                ProductId = 2,
                Name = "ProductCok",
                IsReturnable = true,
                Brand = brand1
            };

            var productForUser2 = new Product
            {
                ProductId = 3,
                Name = "Product3",
                IsReturnable = true,
                Brand = brand1
            };

            var paymentMethod1 = new PayPal
            {
                Email = "user1@test.com",
                User = user1
            };

            // 4) PurchaseOrders 
            var po1 = new PurchaseOrder
            {
                Id = 1,
                Customer = user1,
                City = "City1",
                Street = "Street2",
                PostalCode = "Postal1",
                NameSurname = "Name1Surname1",
                PaymentMethod = paymentMethod1,
                PaymentMethodId = paymentMethod1.Id,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            var po2 = new PurchaseOrder
            {
                Id = 2,
                Customer = user2,
                City = "City2",
                Street = "Street2",
                PostalCode = "Postal2",
                NameSurname = "Name2Surname2",
                PaymentMethod = paymentMethod1,
                PaymentMethodId = paymentMethod1.Id,
                PurchaseProducts = new List<PurchaseProduct>()
            };

            // 5) PurchaseProducts
            var pp_user1 = new PurchaseProduct
            {
                PurchaseOrder = po1,
                PurchaseOrderId = po1.Id,
                Product = productForUser1,
                ProductId = productForUser1.ProductId,
                Quantity = 10,
                // ReturnProduct = null
            };

            var pp_user2 = new PurchaseProduct
            {
                PurchaseOrder = po2,
                PurchaseOrderId = po2.Id,
                Product = productForUser2,
                ProductId = productForUser2.ProductId,
                Quantity = 5
                // Existing ReturnProduct
            };

            po1.PurchaseProducts.Add(pp_user1);
            po2.PurchaseProducts.Add(pp_user2);

            // 6) ReturnPurchaseOrder + ReturnProduct (for pp_user2)
            var rpo2 = new ReturnPurchaseOrder
            {
                Id = 1,
                Customer = user2,
                Name = "Return1",
                ReturnProducts = new List<ReturnProduct>()
            };

            var rp_user2 = new ReturnProduct
            {
                Id = 1,
                PurchaseOrderId = po2.Id,
                ProductId = productForUser2.ProductId,
                PurchaseProduct = pp_user2,
                Reason = "First Reason",
                ReturnPurchaseOrder = rpo2
            };

            rpo2.ReturnProducts.Add(rp_user2);

            // Connecting ReturnPurchaseOrder to ReturnProduct
            pp_user2.ReturnProduct = rp_user2;

            //Context
            _context.Users.AddRange(user1, user2);
            _context.Brands.Add(brand1);
            _context.Products.AddRange(productForUser1, productForUser2);
            _context.PurchaseOrders.AddRange(po1, po2);
            _context.PurchaseProducts.AddRange(pp_user1, pp_user2);
            _context.Returns.Add(rpo2);
            _context.ReturnProducts.Add(rp_user2);
            _context.PaymentMethods.Add(paymentMethod1);

            _context.SaveChanges();
        }

        // ==================== MemberData ====================

        public static IEnumerable<object[]> TestCasesFor_GetPurchaseProductsForReturning_OK()
        {


            var expectedForUser1_All = new List<PurchaseProductsForReturningDTO>()
            {
                new PurchaseProductsForReturningDTO(
                    2,              // ProductId
                    "ProductCok",   // Product.Name
                    10,             // Quantity
                    "Brand1",       // Brand.Name
                    "Location1",    // Brand.Location
                    true            // IsReturnable
                )
            };

            var expectedForUser1_FilterByName = new List<PurchaseProductsForReturningDTO>()
            {
                expectedForUser1_All[0]
            };

            var expectedForUser2_Empty = new List<PurchaseProductsForReturningDTO>();

            var allTests = new List<object[]>
            {
                // productName, quantity, userName, expectedList

                new object[] { null, (int?)null, "TheUser1", expectedForUser1_All },

                new object[] { "Cok", (int?)null, "TheUser1", expectedForUser1_FilterByName },

                new object[] { null, (int?)null, "TheUser2", expectedForUser2_Empty }
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
