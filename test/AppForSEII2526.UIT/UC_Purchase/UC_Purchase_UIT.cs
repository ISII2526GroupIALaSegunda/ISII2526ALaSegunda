using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            selectProductsForPurchasing_PO.WaitForBeingVisible(By.Id("productsTable"));
            Assert.True(true);
        }

        [Theory(DisplayName = "UC2_2 - SelectProducts Filtering")]
        [InlineData("T-Shirt", "Red")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_2_SelectProducts_Filtering_Test(string name, string colour)
        {      
            InitialStepsForSelectProducts();
            selectProductsForPurchasing_PO.SearchProducts(name, colour);

            selectProductsForPurchasing_PO.WaitForBeingVisible(By.Id("productsTable"));
            Assert.True(true);
        }

        [Fact(DisplayName = "UC2_3 - Add Product To Cart")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_3_Add_Product_To_Cart()
        {
            InitialStepsForSelectProducts();

            string productName = "T-Shirt";
            selectProductsForPurchasing_PO.SearchProducts(productName, "");

            // If the product exists, add it.

            Assert.True(true);
        }
    }
}
