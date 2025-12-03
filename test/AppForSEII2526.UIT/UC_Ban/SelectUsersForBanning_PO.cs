using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_Ban
{
    public class SelectUsersForBanning_PO : PageObject
    {
        By banUser = By.Id("BanUser");
        //other input actions
        public SelectUsersForBanning_PO(IWebDriver driver, ITestOutputHelper output) : base(driver, output)
        {
        }
        public void ClickBanUser(string username)
        {
           WaitForBeingClickable(banUser);
            _driver.FindElement(banUser).SendKeys(username);
           // completar : _driver.FindElement().Click();
        }
    }
}
