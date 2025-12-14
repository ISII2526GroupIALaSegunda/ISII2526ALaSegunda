using System;
using System.Collections.Generic;
using System.Globalization;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using System.Threading;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_Purchase_UIT_Extra : UC_UIT
    {
        private SelectProductsForPurchasing_PO selectProductsForPurchasing_PO;

        public UC_Purchase_UIT_Extra(ITestOutputHelper output) : base(output)
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

        [Fact(DisplayName = "UC2_4 - SelectProducts No Results")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_4_SelectProducts_NoResults()
        {
            InitialStepsForSelectProducts();

            selectProductsForPurchasing_PO.SearchProducts("ZapatoInvisible", "Verde");
            Thread.Sleep(1000);

            var tbody = _driver.FindElement(By.Id("productsTable")).FindElement(By.TagName("tbody"));
            var rows = tbody.FindElements(By.TagName("tr"));

            bool isEmpty = rows.Count == 0;
            bool showsMessage = rows.Count > 0 && (rows[0].Text.Contains("No products") || rows[0].Text.Contains("No se encontraron"));
            Assert.True(isEmpty || showsMessage, $"Expected table empty or 'No products', but found {rows.Count} rows.");
        }

        [Fact(DisplayName = "UC2_5 - Check Cart Content After Add")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_5_Check_Cart_Content()
        {
            InitialStepsForSelectProducts();

            string productName = "Jacket";
            selectProductsForPurchasing_PO.SearchProducts(productName, "");
            selectProductsForPurchasing_PO.AddProductToCart(productName);

            selectProductsForPurchasing_PO.WaitForBeingVisible(By.Id("btnPurchase"));

            var card = _driver.FindElement(By.CssSelector(".card"));
            Assert.Contains(productName, card.Text);

            var cardText = card.Text;

            bool foundTotal = cardText.Contains("Total:") || cardText.Contains("Total");
            Assert.True(foundTotal, "The cart does not contain the word 'Total'");
        }

        [Fact(DisplayName = "UC2_6 - Finalize Purchase")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC2_6_Finalize_Purchase()
        {
            InitialStepsForSelectProducts();

            string productName = "Jacket";
            selectProductsForPurchasing_PO.SearchProducts(productName, "");
            selectProductsForPurchasing_PO.AddProductToCart(productName);

            selectProductsForPurchasing_PO.WaitForBeingVisible(By.Id("btnPurchase"));
            selectProductsForPurchasing_PO.ClickPurchaseProducts();

            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            bool navigated = wait.Until(d => d.Url.Contains("/purchases/createpurchase"));


            Assert.True(navigated, "Clicking 'Purchase products' should navigate to the create purchase page.");
        }
    }
}