using AppForMovies.UIT.Shared;
using AppForSEII2526.UIT.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppForSEII2526.UIT.UC_PurchaseDelivery
{
    public class UC_PurchaseDeliveries_UIT : UC_UIT
    {
        private SelectPurchaseOrdersForDelivery_PO selectPurchaseOrdersForDelivery_PO;
        public UC_PurchaseDeliveries_UIT(ITestOutputHelper output) : base(output)
        {

        }

        private void Precondition_perform_login()
        {
            Perform_login("alex@uclm.es", "Password1234%");
        }

        private void InitialStepsForPurchaseDelivery()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("CreateDeliveryAssignment"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateDeliveryAssignment")).Click();
        }
    }
}
