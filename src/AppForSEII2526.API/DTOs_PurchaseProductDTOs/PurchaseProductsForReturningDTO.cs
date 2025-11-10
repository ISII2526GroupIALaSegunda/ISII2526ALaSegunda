namespace AppForSEII2526.API.DTOs_PurchaseProductDTOs
{
    public class PurchaseProductsForReturningDTO
    {
        public PurchaseProductsForReturningDTO(int id, string productName, int quantity, string branchName, string branchLocation, bool isReturnable)
        {
            Id = id;
            Productname = productName;
            Quantity = quantity;
            Branchname = branchName;
            Branchlocation = branchLocation;
            IsReturnable = isReturnable;
        }

        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage = "Product name cannot be longer than 50 characters.")]
        public string Productname { get; set; }
        public int Quantity { get; set; }
        
        [StringLength(50, ErrorMessage = "Branch name cannot be longer than 50 characters.")]
        public string Branchname { get; set; }
        
        public string Branchlocation { get; set; }
        
        public bool IsReturnable { get; set; }
    }
}
