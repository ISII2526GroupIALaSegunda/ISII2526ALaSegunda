using AppForSEII2526.UT; // tu base SQLite en memoria (equivalente a AppForMovies4SqliteUT)
using AppForSEII2526.API.Controllers;
using AppForSEII2526.API.DTOs.ProductDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace AppForSEII2526.UT.ProductsController_test
{
    public class GetProductsForPurchasing_test : AppForSEII2526SqliteUT
    {
        public GetProductsForPurchasing_test()
        {
            var brands = new List<Brand>() {
                new Brand { Id = 1, Name = "Zara",   Location = "Madrid"    },
                new Brand { Id = 2, Name = "Uniqlo", Location = "Barcelona" }
            };

            var products = new List<Product>(){
                new Product { ProductId = 1, Name = "Jacket", Colour = "Red",  Price = 20.0m, Stock = 4,  IsReturnable = true,  Brand = brands[0] },
                new Product { ProductId = 2, Name = "Shirt",  Colour = "Blue", Price = 10.0m, Stock = 10, IsReturnable = false, Brand = brands[1] },

                // this product has stock=0 so it shouldn't be returned when calling GetProductsForPurchasing
                new Product { ProductId = 3, Name = "Hat",    Colour = "Red",  Price =  5.0m, Stock = 0,  IsReturnable = false, Brand = brands[1] },
            };

            _context.AddRange(brands);
            _context.AddRange(products);
            _context.SaveChanges();
        }

        public static IEnumerable<object[]> TestCasesFor_GetProductsForPurchasing_OK()
        {
            // ¡OJO! El endpoint ordena por Colour (OrderBy Colour).
            // Colores: "Blue" < "Red" alfabéticamente, por eso "Shirt" va antes que "Jacket".
            var productDTOs = new List<ProductForPurchaseDTO>() {
                new ProductForPurchaseDTO(2, "Shirt",  "Blue", "Uniqlo", 10, null),
                new ProductForPurchaseDTO(1, "Jacket", "Red",  "Zara",    4, null),
            };

            var all = new List<ProductForPurchaseDTO>() { productDTOs[0], productDTOs[1] }; // los dos (sin "Hat")
            var onlyRed = new List<ProductForPurchaseDTO>() { productDTOs[1] };            // solo "Jacket"
            var onlyByName = new List<ProductForPurchaseDTO>() { productDTOs[0] };          // solo "Shirt" (name contains "irt")

            var tests = new List<object[]>
            {
                new object[] { null,   null,  all      }, // sin filtros → ambos con stock>0, ordenados por Colour
                new object[] { "Red",  null,  onlyRed  }, // filtro por colour → "Jacket"
                new object[] { null,   "irt", onlyByName },// filtro por name substring → "Shirt"
            };

            return tests;
        }

        [Theory]
        [Trait("LevelTesting", "Unit Testing")]
        [MemberData(nameof(TestCasesFor_GetProductsForPurchasing_OK))]
        public async Task GetProductsForPurchasing_filter_test(string? colour, string? name, List<ProductForPurchaseDTO> expected)
        {
            var controller = new ProductsController(_context, new Mock<ILogger<ProductsController>>().Object);

            var result = await controller.GetProductsForPurchasing(colour, name);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var actual = Assert.IsType<List<ProductForPurchaseDTO>>(okResult.Value);

            Assert.Equal(expected, actual);
        }
    }
}
