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

        private const int purchaseOrderId1 = 4;
        private const string purchaseOrderCity1 = "Ab";
        private const string purchaseOrderStreet1 = "Av españa";
        private const string purchaseOrderPostalCode1 = "02002";
        private const string purchaseOrderDate1 = "13/10/2025 0:00:00 +02:00";
        private const string purchaseOrderTotalPrice1 = "5";

        public UC_PurchaseDeliveries_UIT(ITestOutputHelper output) : base(output)
        {
            selectPurchaseOrdersForDelivery_PO = new SelectPurchaseOrdersForDelivery_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("alejandro.gomez31@alu.uclm.es", "Alejandro@1234");
        }

        private void InitialStepsForPurchaseDelivery()
        {
            Precondition_perform_login();
            //we wait for the option of the menu to be visible
            selectPurchaseOrdersForDelivery_PO.WaitForBeingVisible(By.Id("CreateDeliveryAssignment"));
            //we click on the menu
            _driver.FindElement(By.Id("CreateDeliveryAssignment")).Click();
        }

        [Fact(DisplayName = "UC1_BF_AF0 – SelectPurchaseOrdersForDelivery Filtering")]
        [Trait("UseCase", "UC1_BF_AF0")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_BF_AF0_SelectPurchaseOrdersForDelivery_Filtering_Test()
        {
            //Arrange
            InitialStepsForPurchaseDelivery();
            var expectedPurchaseOrders = new List<string[]> {
                new string[] { purchaseOrderId1.ToString(), purchaseOrderDate1, purchaseOrderTotalPrice1, purchaseOrderCity1, purchaseOrderStreet1, purchaseOrderPostalCode1 }
            };

            //Act
            selectPurchaseOrdersForDelivery_PO.SearchPurchaseOrders("purchaseOrderPostalCode1", "purchaseOrderTotalPrice1");

            //Assert
            Assert.True(selectPurchaseOrdersForDelivery_PO.CheckListOfPurchaseOrders(expectedPurchaseOrders));
        }

        [Fact(DisplayName = "UC1_AF1 – GetPurchaseOrdersToDelivery No Results")]
        [Trait("LevelTesting", "Funcional Testing")]
        public void UC1_AF1_Purchase_Order_Not_Available()
        {
            //Arrange
            InitialStepsForPurchaseDelivery();
            //Act
            selectPurchaseOrdersForDelivery_PO.AddPurchaseOrderToDeliveryList(purchaseOrderId1.ToString());
            selectPurchaseOrdersForDelivery_PO.RemovePurchaseOrderToDeliveryList(purchaseOrderId1.ToString());

            //Assert

            Assert.True(selectPurchaseOrdersForDelivery_PO.DeliverNotAvailable());

        }
    }
}
