using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class SelectMoviesForRental_PO : PageObject
    {
        By inputPostalcode = By.Id("inputPostalcode");
        By inputTotalPrice = By.Id("inputTotalPrice");
        By buttonSearchPurchaseOrders = By.Id("searchPurchaseOrders");
        public SelectMoviesForRental_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void SearchPurchaseOrders(string postalcode, string totalPrice)
        {
            //wait for the webelement to be clickable
            WaitForBeingClickable(inputPostalcode);
            _driver.FindElement(inputPostalcode).SendKeys(postalcode);
            _driver.FindElement(inputTotalPrice).SendKeys(totalPrice);
            _driver.FindElement(buttonSearchPurchaseOrders).Click();


        }
    }
}
