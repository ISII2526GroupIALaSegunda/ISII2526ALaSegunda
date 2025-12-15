using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;
using System.Data;

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

        By inputCustomerName = By.Id("NameCustomer");
        By inputCustomerSurname = By.Id("SurnameCustomer");
        By inputStreet = By.Id("Street");
        By inputCity = By.Id("City");
        By inputPostalCode = By.Id("PostalCode");
        By selectPaymentMethod = By.Id("PaymentMethodId");
        By btnConfirmPurchase = By.Id("btnConfirmPurchase");
        By totalPrice = By.Id("TotalPrice");

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

        public void FillPurchaseForm(string name, string surname, string street, string city, string zip)
        {
            WaitForBeingVisible(inputCustomerName);
            WaitForBeingVisible(inputCustomerSurname);
            WaitForBeingVisible(inputStreet);
            WaitForBeingVisible(inputCity);
            WaitForBeingVisible(inputPostalCode);

            _driver.FindElement(inputCustomerName).Clear();
            _driver.FindElement(inputCustomerName).SendKeys(name);

            _driver.FindElement(inputCustomerSurname).Clear();
            _driver.FindElement(inputCustomerSurname).SendKeys(surname);

            _driver.FindElement(inputStreet).Clear();
            _driver.FindElement(inputStreet).SendKeys(street);

            _driver.FindElement(inputCity).Clear();
            _driver.FindElement(inputCity).SendKeys(city);

            _driver.FindElement(inputPostalCode).Clear();
            _driver.FindElement(inputPostalCode).SendKeys(zip);
        }

        public void SelectFirstAvailablePaymentMethod()
        {
            WaitForBeingVisible(selectPaymentMethod);
            var select = new SelectElement(_driver.FindElement(selectPaymentMethod));

            if (select.Options.Count <= 1)
                throw new InvalidOperationException("No payment methods available for selection.");

            select.SelectByIndex(1);
        }

        public void DecreaseItems()
        {
            var removeButton = _driver.FindElement(By.CssSelector("button.btn.outline-danger"));
        }

        public void SubmitPurchaseForm()
        {
            WaitForBeingVisible(btnConfirmPurchase);
            WaitForBeingClickable(btnConfirmPurchase);
            _driver.FindElement(btnConfirmPurchase).Click();
        }

        public bool IsPurchaseCreated()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            bool navigated = wait.Until(d => d.Url.Contains("/purchase/detailpurchase") && d.Url.Contains("PurchaseID="));
            if (!navigated)
                return false;

            WaitForBeingVisible(totalPrice);
            return true;
        }
    }
}
