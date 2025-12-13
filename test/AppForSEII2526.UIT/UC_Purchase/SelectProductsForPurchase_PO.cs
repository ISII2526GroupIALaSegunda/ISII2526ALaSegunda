using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using Xunit.Abstractions;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectProductsForPurchase_PO : PageObject
    {
        private By inputNameBy = By.Id("inputName");

        private By inputColourBy = By.Id("inputColour");
        private By btnSearchBy = By.Id("btnSearch");
        private By tableProductsBy = By.Id("productsTable");

        private By errorBoxBy = By.Id("errorBox");
        private By btnPurchaseBy = By.Id("btnPurchase");

        public SelectProductsForPurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchProducts(string name, string colour)
        {
            WaitForBeingClickable(inputNameBy);
            if (!string.IsNullOrEmpty(name))
            {
                _driver.FindElement(inputNameBy).Clear();
                _driver.FindElement(inputNameBy).SendKeys(name);
            }

            if (!string.IsNullOrEmpty(colour))
            {
                _driver.FindElement(inputColourBy).Clear();
                _driver.FindElement(inputColourBy).SendKeys(colour);
            }

            _driver.FindElement(btnSearchBy).Click();
            Thread.Sleep(500); 
        }

        // Verificación: Comprobar contenido de la tabla
        public bool CheckListOfProducts(List<string[]> expectedProducts)
        {
            return CheckBodyTable(expectedProducts, tableProductsBy);
        }

        // Acción: Añadir al carrito
        public void AddProductToCart(string productName)
        {
            By btnAddBy = By.Id($"btnAdd_{productName}");
            WaitForBeingClickable(btnAddBy);
            _driver.FindElement(btnAddBy).Click();
        }

        public bool IsPurchaseButtonVisible()
        {
            try
            {
                return _driver.FindElement(btnPurchaseBy).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
        public bool CheckErrorMessage(string expectedError)
        {
            WaitForBeingVisible(errorBoxBy);
            return _driver.FindElement(errorBoxBy).Text.Contains(expectedError);
        }
    }
}
