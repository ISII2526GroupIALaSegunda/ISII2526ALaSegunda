using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class PurchaseDetail_PO : PageObject
    {
        private readonly By nameSurname = By.Id("NameSurname");
        private readonly By deliveryAddress = By.Id("DeliveryAddress");
        private readonly By paymentMethod = By.Id("PaymentMethod");
        private readonly By purchaseDate = By.Id("PurchaseDate");
        private readonly By state = By.Id("State");
        private readonly By totalPrice = By.Id("TotalPrice");

        private readonly By purchasedProductsTable = By.Id("PurchasedProducts");

        public PurchaseDetail_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }

        private By PurchaseItemRowByName(string productName) => By.Id($"PurchaseItem_{productName}");

        public bool WaitUntilOnPage()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(15));
            return wait.Until(d => d.Url.Contains("/purchase/detailpurchase") && d.Url.Contains("PurchaseID="));
        }

        public void WaitForLoaded()
        {
            WaitForBeingVisible(nameSurname);
            WaitForBeingVisible(deliveryAddress);
            WaitForBeingVisible(paymentMethod);
            WaitForBeingVisible(purchaseDate);
            WaitForBeingVisible(state);
            WaitForBeingVisible(purchasedProductsTable);
            WaitForBeingVisible(totalPrice);
        }

        public bool CheckCustomerAndAddress(string expectedNameSurname, string expectedStreet, string expectedCity, string expectedPostalCode)
        {
            WaitForLoaded();

            var actualNameSurname = _driver.FindElement(nameSurname).Text ?? string.Empty;
            var actualDeliveryAddress = _driver.FindElement(deliveryAddress).Text ?? string.Empty;

            bool ok = true;
            ok = ok && actualNameSurname.Contains(expectedNameSurname);
            ok = ok && actualDeliveryAddress.Contains(expectedStreet);
            ok = ok && actualDeliveryAddress.Contains(expectedCity);
            ok = ok && actualDeliveryAddress.Contains(expectedPostalCode);
            return ok;
        }

        public bool PaymentMethodIsNotEmpty()
        {
            WaitForLoaded();
            var value = _driver.FindElement(paymentMethod).Text;
            return !string.IsNullOrWhiteSpace(value);
        }

        public bool HasPurchasedProduct(string productName)
        {
            WaitForLoaded();
            WaitForBeingVisible(PurchaseItemRowByName(productName));
            return _driver.FindElement(PurchaseItemRowByName(productName)).Displayed;
        }
    }
}
