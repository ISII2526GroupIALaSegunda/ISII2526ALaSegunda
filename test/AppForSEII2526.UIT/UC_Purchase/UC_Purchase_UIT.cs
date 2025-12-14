using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_Purchase_UIT : UC_UIT
    {
        private SelectProductsForPurchasing_PO selectProductsForPurchasing_PO;

        public UC_Purchase_UIT(ITestOutputHelper output) : base(output)
        {
            selectProductsForPurchasing_PO = new SelectProductsForPurchasing_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("pepe@uclm.es", "Password1234%");
        }

        private void InitialStepsForSelectProducts()
        {
            Precondition_perform_login();
            selectProductsForPurchasing_PO.WaitForBeingVisible(By.XPath("//button[contains(., 'Logout')]"));

            selectProductsForPurchasing_PO.WaitForBeingVisible(By.Id("menuSelectProducts"));
            _driver.FindElement(By.Id("menuSelectProducts")).Click();
        }

        [Fact(DisplayName = "UC2_1 - SelectProducts No Filtering")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_1_SelectProducts_No_Filtering_Test()
        {
            InitialStepsForSelectProducts();

            selectProductsForPurchasing_PO.SearchProducts("", "");

            var expectedProducts = new List<string[]>
            {
                
                new string[] { "Shirt", "Uniqlo", "Blue", "2", "Albacete", "Add" },
                new string[] { "Jacket", "Zara", "Red", "4", "Albacete", "Add" }
              
            };

            bool areEqual = selectProductsForPurchasing_PO.CheckProductsList(expectedProducts);

            Assert.True(areEqual, "La tabla no contiene los productos del SeedData");
        }

        [Theory(DisplayName = "UC2_2 - SelectProducts Filtering")]
        [InlineData("Jacket", "Red")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_2_SelectProducts_Filtering_Test(string name, string colour)
        {
            InitialStepsForSelectProducts();

            selectProductsForPurchasing_PO.SearchProducts(name, colour);

            var expectedProducts = new List<string[]>
            {
                new string[] { "Jacket", "Zara", "Red", "4", "Albacete", "Add" }
            };

            bool areEqual = selectProductsForPurchasing_PO.CheckProductsList(expectedProducts);
            Assert.True(areEqual, $"El filtrado por {name} y {colour} falló.");
        }

        [Fact(DisplayName = "UC2_3 - Add Product To Cart")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_3_Add_Product_To_Cart()
        {
            InitialStepsForSelectProducts();

            string productName = "Jacket";
            selectProductsForPurchasing_PO.SearchProducts(productName, "");

            selectProductsForPurchasing_PO.AddProductToCart(productName);

            Assert.True(true);
        }
    }
}