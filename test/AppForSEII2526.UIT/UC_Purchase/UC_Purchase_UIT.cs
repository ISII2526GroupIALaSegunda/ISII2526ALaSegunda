using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;
using OpenQA.Selenium;
using AppForMovies.UIT.Shared;

using AppForSEII2526.UIT.Shared;

namespace AppForSEII2526.UIT.UC_Purchase
{
    public class UCPurchase_UIT : UC_UIT
    {
        private readonly SelectProductsForPurchasing_PO selectProducts_PO;
        private readonly CreatePurchase_PO createPurchase_PO;
        private readonly PurchaseDetail_PO purchaseDetail_PO;

        public UCPurchase_UIT(ITestOutputHelper output) : base(output)
        {
            Initial_step_opening_the_web_page();
            selectProducts_PO = new SelectProductsForPurchasing_PO(_driver, _output);
            createPurchase_PO = new CreatePurchase_PO(_driver, _output);
            purchaseDetail_PO = new PurchaseDetail_PO(_driver, _output);
        }


        private void Precondition_LoginAndNavigate()
        {
            Perform_login("pepe@uclm.es", "Password1234%"); 
            selectProducts_PO.WaitForBeingVisible(By.XPath("//button[contains(., 'Logout')]"));
            selectProducts_PO.OpenFromMenu(); 
        }


        private void Helper_AddProductAndProceed(string productName)
        {
            selectProducts_PO.SearchProducts(productName, "");
            selectProducts_PO.AddProductToCart(productName); 
            selectProducts_PO.ClickPurchaseProducts();       
        }


        //Main flow
        
        [Fact(DisplayName = "Main Flow - Successful Purchase")]
        [Trait("LevelTesting", "Functional Testing")]
        public void MainFlow_SuccessfulPurchase()
        {

            Precondition_LoginAndNavigate();

            string productName = "Jacket";
            Helper_AddProductAndProceed(productName);

            
            Assert.True(createPurchase_PO.IsOnCreatePurchasePage(), "Should be on Create Purchase page");

            createPurchase_PO.FillPurchaseForm("Pepe", "Perez", "Calle Real, 1", "Albacete", "02001");
            createPurchase_PO.SelectFirstAvailablePaymentMethod();
            createPurchase_PO.Submit();
            Assert.True(purchaseDetail_PO.WaitUntilOnPage(), "Should navigate to Details page");

            Assert.True(purchaseDetail_PO.CheckCustomerAndAddress("Pepe Perez", "Calle Real, 1", "Albacete", "02001"),
                "Customer data or address incorrect in details");
            Assert.True(purchaseDetail_PO.HasPurchasedProduct(productName),
                "Purchased product missing in details");
        }


        //AF0
        [Fact(DisplayName = "Alternative Flow 0 - No products warning")]
        [Trait("LevelTesting", "Functional Testing")]
        public void AF0_NoProductsToPurchase()
        {
            Precondition_LoginAndNavigate();
            selectProducts_PO.SearchProducts("NonExistentProductXYZ", "");
            Assert.True(selectProducts_PO.IsNoProductsResultShown(),
                "System should warn the user when no products are found");
        }


        //AF1
        [Theory(DisplayName = "Alternative Flow 1 - Filtering Products")]
        [InlineData("Jacket", "Red")]
        [Trait("LevelTesting", "Functional Testing")]
        public void AF1_FilteringProducts(string name, string colour)
        {
            Precondition_LoginAndNavigate();
            selectProducts_PO.SearchProducts(name, colour);

            var expectedProducts = new List<string[]>
            {
                new string[] { "Jacket", "Zara", "Red", "4", "Albacete", "Add" }
            };
            Assert.True(selectProducts_PO.CheckProductsList(expectedProducts),
                "Filtering did not return expected products");
        }

        [Fact(DisplayName = "Alternative Flow 1 - Filtering No Results")]
        public void AF1_Filtering_NoResults()
        {
            Precondition_LoginAndNavigate();
            selectProducts_PO.SearchProducts("NonExistent", "Pink");
            Assert.True(selectProducts_PO.IsNoProductsResultShown(),
                "Should show empty table or message");
        }

        //AF3

        [Fact(DisplayName = "Alternative Flow 3 - Empty Cart Button Disabled/Hidden")]
        [Trait("LevelTesting", "Functional Testing")]
        public void AF3_EmptyCart_PurchaseUnavailable()
        {
            Precondition_LoginAndNavigate();
            Assert.False(selectProducts_PO.IsPurchaseButtonEnabled(),
                "Purchase button should be disabled when cart is empty");
        }

        //AF5

        [Theory(DisplayName = "Alternative Flow 5 - Validation Errors")]
        [InlineData("", "Perez", "Street 1", "City", "00000", "The NameCustomer field is required.")]
        [InlineData("Pepe", "", "Street 1", "City", "00000", "The SurnameCustomer field is required.")]
        [Trait("LevelTesting", "Functional Testing")]
        public void AF5_ValidationErrors(string name, string surname, string street, string city, string zip, string expectedError)
        {
            Precondition_LoginAndNavigate();
            Helper_AddProductAndProceed("Jacket");
            createPurchase_PO.FillPurchaseForm(name, surname, street, city, zip);
            createPurchase_PO.Submit();

            Assert.True(createPurchase_PO.IsOnCreatePurchasePage(), "Should stay on page");
            Assert.True(createPurchase_PO.HasValidationError(expectedError),
                $"Expected error message '{expectedError}' was not found in the page source.");
        }
    }
}