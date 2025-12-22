using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class CreatePurchase_PO : PageObject
    {
        private readonly By inputCustomerName = By.Id("NameCustomer");
        private readonly By inputCustomerSurname = By.Id("SurnameCustomer");
        private readonly By inputStreet = By.Id("Street");
        private readonly By inputCity = By.Id("City");
        private readonly By inputPostalCode = By.Id("PostalCode");
        private readonly By selectPaymentMethod = By.Id("PaymentMethodId");
        private readonly By btnConfirmPurchase = By.Id("btnConfirmPurchase");
        private readonly By validationSummary = By.CssSelector("div.validation-summary-valid, div.validation-summary-errors");

        public CreatePurchase_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        public bool IsOnCreatePurchasePage()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
            return wait.Until(d => d.Url.Contains("/purchases/createpurchase"));
        }

        public void WaitForLoaded()
        {
            WaitForBeingVisible(inputCustomerName);
            WaitForBeingVisible(inputCustomerSurname);
            WaitForBeingVisible(inputStreet);
            WaitForBeingVisible(inputCity);
            WaitForBeingVisible(inputPostalCode);
            WaitForBeingVisible(selectPaymentMethod);
        }

        public void FillPurchaseForm(string name, string surname, string street, string city, string zip)
        {
            WaitForLoaded();

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

        public void Submit()
        {
            WaitForBeingVisible(btnConfirmPurchase);
            WaitForBeingClickable(btnConfirmPurchase);
            _driver.FindElement(btnConfirmPurchase).Click();
        }

        // helper to check validation summary contains expected message
        public bool HasValidationError(string expectedError)
        {
            try
            {
                var byText = By.XPath($"//*[contains(text(), '{expectedError}')]");
                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.ElementIsVisible(byText));
                return true;
            }
            catch (WebDriverTimeoutException)
            {
                return false;
            }
        }
    }
}
