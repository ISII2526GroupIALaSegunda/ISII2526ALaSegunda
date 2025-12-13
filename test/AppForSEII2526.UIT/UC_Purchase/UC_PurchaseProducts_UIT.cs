using AppForMovies.UIT.Shared;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UC_PurchaseProducts_UIT : UC_UIT
    {
        private SelectProductsForPurchase_PO _selectProductsPO;

        public UC_PurchaseProducts_UIT(ITestOutputHelper output) : base(output)
        {
            _selectProductsPO = new SelectProductsForPurchase_PO(_driver, _output);
        }

        private void InitialSteps()
        {
            Perform_login("hector@uclm.es", "Password1234%");
            _selectProductsPO.WaitForBeingVisible(By.Id("menuSelectProducts"));
            _driver.FindElement(By.Id("menuSelectProducts")).Click();
        }

        [Theory]
        [Trait("LevelTesting", "Funcional Testing")]
        [InlineData("Jacket", "", "Jacket", "Zara", "Red", "4")] 
        public void UC_FilterProducts_Test(string searchName, string searchColour, 
                                           string expName, string expBrand, string expColour, string expStock)
        {
            InitialSteps();
            var expectedRows = new List<string[]> 
            { 
                new string[] { expName, expBrand, expColour, expStock } 
            };

            _selectProductsPO.SearchProducts(searchName, searchColour);

            Assert.True(_selectProductsPO.CheckListOfProducts(expectedRows), 
                $"La tabla no contiene el producto esperado: {expName}");
        }

        [Fact]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC_AddProductToCart_EnablesButton_Test()
        {
            InitialSteps();
            string productToAdd = "Jacket";

            _selectProductsPO.SearchProducts(productToAdd, "");
            _selectProductsPO.AddProductToCart(productToAdd);

            Assert.True(_selectProductsPO.IsPurchaseButtonVisible(), 
                "El botón de Confirmar Compra debería ser visible tras añadir un producto.");
        }
    }
}
