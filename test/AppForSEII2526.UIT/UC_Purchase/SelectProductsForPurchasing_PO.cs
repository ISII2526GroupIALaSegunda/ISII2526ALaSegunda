using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class SelectProductsForPurchasing_PO : PageObject
    {
        By inputName = By.Id("inputName");
        By inputColour = By.Id("inputColour");
        By btnSearch = By.Id("btnSearch");
        By productsTable = By.Id("productsTable");
        By btnPurchase = By.Id("btnPurchase");
        By errorBox = By.Id("errorBox");

        public SelectProductsForPurchasing_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public void SearchProducts(string name, string colour)
        {
            WaitForBeingVisible(inputName);
            WaitForBeingClickable(inputName);
            WaitForBeingVisible(inputColour);

            _driver.FindElement(inputName).Clear();
            _driver.FindElement(inputName).SendKeys(name);

            _driver.FindElement(inputColour).Clear();
            _driver.FindElement(inputColour).SendKeys(colour);

            WaitForBeingClickable(btnSearch);
            _driver.FindElement(btnSearch).Click();
        }

        public void AddProductToCart(string productName)
        {
            string btnId = "btnAdd_" + productName;
            WaitForBeingVisible(By.Id(btnId));
            WaitForBeingClickable(By.Id(btnId));
            _driver.FindElement(By.Id(btnId)).Click();
        }

        public bool CheckProductsList(List<string[]> expectedProducts)
        {
            WaitForBeingVisible(productsTable);
            return CheckBodyTable(expectedProducts, productsTable);
        }

        public bool CheckError(string expectedError)
        {
            WaitForBeingVisible(errorBox);
            string errorText = _driver.FindElement(errorBox).Text;
            return errorText.Contains(expectedError);
        }

        public void ClickPurchaseProducts()
        {
            WaitForBeingVisible(btnPurchase);
            WaitForBeingClickable(btnPurchase);
            _driver.FindElement(btnPurchase).Click();
        }
    }
}
