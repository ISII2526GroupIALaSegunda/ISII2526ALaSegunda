using AppForMovies.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UCPurchaseExam : UC_UIT

    {
        private SelectProductsForPurchasing_PO selectProducts_PO;
        private CreatePurchase_PO createPurchase_PO;
        private PurchaseDetail_PO purchaseDetail_PO;

        public UCPurchaseExam(ITestOutputHelper output) : base(output)
        {
            selectProducts_PO = new SelectProductsForPurchasing_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
            purchaseDetail_PO = new PurchaseDetail_PO(_driver, _output);
        }

        private void Precondition_perform_login()
        {
            Perform_login("pepe@uclm.es", "Password1234%");

        }
        private void InitialStepsForPurchase()
        {
            Precondition_perform_login();

            try
            {
                System.Threading.Thread.Sleep(1000);
                selectProducts_PO.OpenFromMenu();
            }
            catch (OpenQA.Selenium.StaleElementReferenceException)
            {
                selectProducts_PO.OpenFromMenu();
            }
        }

        private void AddProductAndProceed(string productName)
        {
            selectProducts_PO.SearchProducts(productName, "");
            selectProducts_PO.AddProductToCart(productName);
            selectProducts_PO.ClickPurchaseProducts();
        }

        [Fact]
        [Trait("LevelTesting", "Functional Testing")]
        public void UC_AF1_FilterAddRemoveAndPurchase()
        {
            InitialStepsForPurchase();
            //Filter by colour
            selectProducts_PO.SearchProducts("", "Blue");
            //Add a new product
            selectProducts_PO.AddProductToCart("Shirt");
            //Filter by name
            selectProducts_PO.SearchProducts("Jacket", "");
            //Add a new product
            selectProducts_PO.AddProductToCart("Jacket");
            //Add a new
            selectProducts_PO.SearchProducts("Shirt", "");
            selectProducts_PO.AddProductToCart("Shirt");

            //Remove product
            selectProducts_PO.DecreaseProductQuantity("Shirt");
            int shirtQuantity = selectProducts_PO.GetProductQuantityInCart("Shirt");
            Assert.True(shirtQuantity == 1, $"Expected Shirt quantity to be 1 but is {shirtQuantity}");

            //Continue the basic flow

            string productName = "Jacket";
            AddProductAndProceed(productName);
            createPurchase_PO.FillPurchaseForm("Pepe", "Perez", "Calle Real, 1", "Albacete", "02001");
            createPurchase_PO.SelectFirstAvailablePaymentMethod();
            createPurchase_PO.Submit();

            Assert.True(purchaseDetail_PO.CheckCustomerAndAddress("Pepe Perez", "Calle Real, 1", "Albacete", "02001"));
            Assert.True(purchaseDetail_PO.HasPurchasedProduct(productName));
            Assert.True(purchaseDetail_PO.PaymentMethodIsNotEmpty());
            Assert.True(purchaseDetail_PO.IsStateDisplayed("Request"));


        }



    }
}

    
